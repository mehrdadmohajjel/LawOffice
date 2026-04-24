using Hangfire;
using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models;
using LawOffice.Services;
using LawOffice.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Admin,Staff,Lawyer")]
    [Route("CourtSessions")]
    public class CourtSessionsController : Controller
    {
        private readonly AppDbContext _db;

        public CourtSessionsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var query = _db.CourtSessions
                .Include(s => s.Case).ThenInclude(c => c.Client)
                .Include(s => s.Lawyer).ThenInclude(l => l.User)
                .AsQueryable();

            if (role == "Lawyer")
            {
                query = query.Where(s => s.Lawyer.UserId == userId);
            }

            // مرحله ۱: دریافت داده‌های خام از دیتابیس (بدون تبدیل تاریخ)
            var dbSessions = await query.OrderByDescending(s => s.SessionDate)
                .Select(s => new
                {
                    s.Id,
                    CaseCode = s.Case.CaseCode,
                    ClientFirstName = s.Case.Client.FirstName,
                    ClientLastName = s.Case.Client.LastName,
                    LawyerFirstName = s.Lawyer.User.FirstName,
                    LawyerLastName = s.Lawyer.User.LastName,
                    s.SessionDate,
                    s.SessionTime,
                    s.CourtName,
                    s.Branch,
                    Status = (int)s.Status,
                    s.Result,
                    s.Notes
                }).ToListAsync();

            // مرحله ۲: انجام عملیات تبدیلات سمت سرور (روی حافظه)
            var sessions = dbSessions.Select(s => new
            {
                s.Id,
                CaseCode = s.CaseCode,
                ClientName = s.ClientFirstName + " " + s.ClientLastName,
                LawyerName = s.LawyerFirstName + " " + s.LawyerLastName,
                SessionDate = s.SessionDate.ToShamsiDateString(), // اکنون این دستور بدون مشکل اجرا می‌شود
                SessionTime = s.SessionTime.ToString(@"hh\:mm"),
                s.CourtName,
                s.Branch,
                s.Status, // مقدار عدد (int) در مرحله قبل آماده شده است
                s.Result,
                s.Notes
            }).ToList();

            return Json(sessions);
        }

        [Route("GetLawyers")]
        [HttpGet]
        public async Task<IActionResult> GetLawyers()
        {
            var lawyers = await _db.Lawyers
                .Select(l => new { l.Id, FullName = l.FirstName + " " + l.LastName })
                .ToListAsync();
            return Ok(lawyers);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CourtSessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "اطلاعات نامعتبر است" });

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var latinDate = model.SessionDate.ConvertToEnglishNumber().ToLatinDate();

                // اصلاح خطای TimeSpan
                // فرض بر این است که فرمت زمان ارسالی معتبر است (مثل "14:30")
                var parsedTime = TimeSpan.Parse(model.SessionTime.ConvertToEnglishNumber());

                var session = new CourtSession
                {
                    CaseId = model.CaseId,
                    LawyerId = model.LawyerId,
                    SessionDate = latinDate,
                    SessionTime = parsedTime, // تخصیص TimeSpan
                    CourtName = model.CourtName,
                    Branch = model.Branch,
                    Status = CourtSessionStatus.Scheduled,
                    Notes = model.Notes
                };

                _db.CourtSessions.Add(session);
                await _db.SaveChangesAsync();

                var caseEntity = await _db.Cases
                    .Include(c => c.Client)
                    .FirstOrDefaultAsync(c => c.Id == model.CaseId);
                var lawyer = await _db.Lawyers.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == model.LawyerId);

                if (caseEntity?.Client != null && lawyer?.User != null)
                {
                    var sessionDateTime = latinDate.Add(parsedTime);
                    var reminderTime = sessionDateTime.AddHours(-24);

    

                    if (reminderTime > DateTime.Now)
                    {
                        var clientReminder = $"جناب/سرکار {caseEntity.Client.FirstName} {caseEntity.Client.LastName}\nیادآوری: جلسه دادگاه شما فردا ساعت {model.SessionTime} در {model.CourtName} می‌باشد.";
                        BackgroundJob.Schedule<ISmsService>(
                            sms => sms.SendAsync(caseEntity.Client.PhoneNumber, clientReminder, "CourtReminder", session.Id),
                            reminderTime - DateTime.Now);

                        var lawyerReminder = $"همکار گرامی\nیادآوری: جلسه دادگاه فردا ساعت {model.SessionTime} مربوط به پرونده {caseEntity.CaseCode} می‌باشد.";
                        BackgroundJob.Schedule<ISmsService>(
                            sms => sms.SendAsync(lawyer.User.PhoneNumber, lawyerReminder, "CourtReminder", session.Id),
                            reminderTime - DateTime.Now);
                    }

                    var clientMessage = $"جلسه دادگاه جدید برای پرونده شما ثبت شد.\nتاریخ: {model.SessionDate}\nساعت: {model.SessionTime}\nشعبه: {model.Branch}";
                    BackgroundJob.Enqueue<ISmsService>(
                        sms => sms.SendAsync(caseEntity.Client.PhoneNumber, clientMessage, "CourtNotification", session.Id));

                    var lawyerMessage = $"جلسه دادگاه جدید ثبت شد.\nپرونده: {caseEntity.CaseCode}\nتاریخ: {model.SessionDate}\nساعت: {model.SessionTime}";
                    BackgroundJob.Enqueue<ISmsService>(
                        sms => sms.SendAsync(lawyer.User.PhoneNumber, lawyerMessage, "CourtNotification", session.Id));
                }

                await transaction.CommitAsync();
                return Ok(new { success = true, message = "جلسه دادگاه با موفقیت ثبت شد" });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { success = false, message = "خطای سرور در ثبت جلسه" });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var session = await _db.CourtSessions.FindAsync(id);
                if (session == null)
                {
                    // ✅ اصلاح: به جای NotFound() خالی، JSON برگردان
                    return NotFound(new { success = false, message = "جلسه دادگاه یافت نشد" });
                }

                _db.CourtSessions.Remove(session);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "جلسه با موفقیت حذف شد" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "خطای سرور در حذف جلسه" });
            }
        }



        [Route("GetForDropdown")]
        [HttpGet]
        public async Task<IActionResult> GetForDropdown()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var query = _db.Cases.Include(c => c.Client).AsQueryable();

            // اگر کاربر وکیل است، فقط پرونده‌های مرتبط با خودش را ببیند
            if (role == "Lawyer")
            {
                // --- خط زیر اصلاح شد ---
                query = query.Where(c =>  c.Lawyer.UserId == userId);
            }

            var cases = await query
                .OrderByDescending(c => c.Id)
                .Select(c => new {
                    id = c.Id,
                    text = c.CaseCode + " - " + c.Client.FirstName + " " + c.Client.LastName
                }).ToListAsync();

            return Json(cases);
        }

        [HttpGet]
        [Route("GetById/{id}")] 
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "شناسه جلسه نامعتبر است." });
            }

            var courtSession = await _db.CourtSessions
                .AsNoTracking() 
                .Where(cs => cs.Id == id)
                .Select(cs => new 
                {
                    cs.Id,
                    cs.CaseId,
                    CaseTitle = cs.Case.Title, 
                    SessionDate = cs.SessionDate.ToString("yyyy-MM-dd"),
                    cs.SessionTime,
                    cs.Branch,
                    cs.CourtName,
                    cs.Notes,
                    cs.LawyerId,
                    LawyerName = cs.Lawyer != null ? (cs.Lawyer.FirstName + " " + cs.Lawyer.LastName) : "بدون وکیل",
                    Status = (int)cs.Status // ارسال مقدار عددی Enum
                })
                .FirstOrDefaultAsync();

            if (courtSession == null)
            {
                // اگر جلسه‌ای با این شناسه یافت نشد، خطای 404 برگردان
                return NotFound(new { success = false, message = "جلسه دادگاه یافت نشد." });
            }

            // اگر اطلاعات با موفقیت یافت شد، آن را با کد 200 OK برگردان
            return Ok(courtSession);
        }
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(long id, [FromBody] CourtSessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "اطلاعات نامعتبر است" });

            try
            {
                var session = await _db.CourtSessions.FindAsync(id);
                if (session == null)
                    return Json(new { success = false, message = "جلسه دادگاه یافت نشد" });

                // تبدیل تاریخ و زمان دقیقا مشابه متد Create
                var latinDate = model.SessionDate.ConvertToEnglishNumber().ToLatinDate();
                var parsedTime = TimeSpan.Parse(model.SessionTime.ConvertToEnglishNumber());

                // به‌روزرسانی فیلدها
                session.CaseId = model.CaseId;
                session.LawyerId = model.LawyerId;
                session.SessionDate = latinDate;
                session.SessionTime = parsedTime;
                session.CourtName = model.CourtName;
                session.Branch = model.Branch;
                session.Notes = model.Notes;


                _db.CourtSessions.Update(session);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "جلسه دادگاه با موفقیت ویرایش شد" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "خطای سرور در ویرایش جلسه" });
            }
        }

    }


}
