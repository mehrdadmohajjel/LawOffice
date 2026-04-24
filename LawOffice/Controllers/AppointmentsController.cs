using Hangfire;
using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models; // Assuming your entities are here
using LawOffice.Services;
using LawOffice.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization; // For PersianCalendar
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin,Staff,Lawyer")]
public class AppointmentsController : Controller
{
    private readonly AppDbContext _db;

    public AppointmentsController(AppDbContext db)
    {
        _db = db;
    }

    // ACTION: Display the main appointments page
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // API: Get all lawyers for dropdown
    [HttpGet]
    public async Task<IActionResult> GetLawyers()
    {
        var lawyers = await _db.Lawyers
            .Select(l => new { l.Id, FullName = l.FirstName + " " + l.LastName })
            .ToListAsync();
        return Ok(lawyers);
    }

    // API: Get all clients for dropdown
    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _db.Clients
            .Select(c => new { c.Id, FullName = c.FirstName + " " + c.LastName })
            .ToListAsync();
        return Ok(clients);
    }

    // API: Get appointments by a specific Jalali date string
    [HttpGet]
    public async Task<IActionResult> GetByDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            return BadRequest(new { success = false, message = "تاریخ ارائه نشده است." });
        }

        try
        {
            var gregorianDate = date.ConvertToEnglishNumber().ToLatinDate();

            // 1. دریافت داده‌ها از دیتابیس (اجرای کوئری SQL بدون فرمت کردن زمان)
            var appointmentsData = await _db.Appointments
                .Where(a => a.AppointmentDate.Date == gregorianDate.Date)
                .Select(a => new
                {
                    a.Id,
                    ClientName = a.Client != null ? a.Client.FirstName + " " + a.Client.LastName : "نامشخص",
                    LawyerName = a.Lawyer != null ? a.Lawyer.FirstName + " " + a.Lawyer.LastName : "نامشخص",
                    a.StartTime, 
                    a.EndTime, 
                    a.Notes,
                    a.Status
                })
                .ToListAsync(); 

            var appointments = appointmentsData
                .OrderBy(a => a.StartTime) // مرتب‌سازی بر اساس زمان
                .Select(a => new
                {
                    a.Id,
                    a.ClientName,
                    a.LawyerName,
                    // اعمال ToString در حافظه با فرمت صحیح TimeSpan
                    AppointmentTime = a.StartTime.ToString(@"hh\:mm"),
                    EndTime= a.EndTime.ToString(@"hh\:mm"),
                    a.Notes,
                    Status = (int)a.Status
                })
                .ToList();

            return Ok(appointments);
        }
        catch (Exception ex)
        {
            // Log the exception (ex)
            return StatusCode(500, new { success = false, message = "خطا در پردازش اطلاعات نوبت‌ها." });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(long id, [FromBody] ChangeStatusRequest request)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment == null) return NotFound("نوبت یافت نشد.");

        appointment.Status = request.Status;
        _db.Appointments.Update(appointment);
        await _db.SaveChangesAsync();

        return Ok(new { message = "وضعیت با موفقیت تغییر کرد." });
    }

    // API: Create a new appointment
    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] AppointmentViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(new { success = false, message = "اطلاعات ارسال شده نامعتبر است." });
    //    }

    //    try
    //    {
    //        var appointment = new Appointment
    //        {
    //            AppointmentDate = model.AppointmentDate.ConvertToEnglishNumber().ToLatinDate(),
    //            StartTime = model.StartTime,
    //            EndTime = model.EndTime,
    //            ClientId = model.ClientId,
    //            LawyerId = model.LawyerId,
    //            Notes = model.Notes,
    //            Status =AppointmentStatus.Reserved
    //        };

    //        _db.Appointments.Add(appointment);
    //        await _db.SaveChangesAsync();

    //        return Ok(new { success = true, message = "نوبت با موفقیت ثبت شد." });
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the exception
    //        return StatusCode(500, new { success = false, message = "خطای سرور هنگام ثبت نوبت." });
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, message = "اطلاعات نامعتبر است" });

        // آغاز تراکنش دیتابیس
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            // تبدیل تاریخ شمسی به میلادی یک بار برای استفاده در کل متد
            var latinDate = model.AppointmentDate.ConvertToEnglishNumber().ToLatinDate();

            var hasConflict = await _db.Appointments.AnyAsync(a =>
                a.LawyerId == model.LawyerId &&
                a.AppointmentDate.Date == latinDate.Date &&
                a.StartTime == model.StartTime &&
                a.Status != AppointmentStatus.Cancelled);

            if (hasConflict)
                return Json(new { success = false, message = "این زمان قبلاً رزرو شده است" });

            var appointment = new Appointment
            {
                LawyerId = model.LawyerId,
                ClientId = model.ClientId,
                CaseId = model.CaseId,
                AppointmentDate = latinDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Status = AppointmentStatus.Reserved,
                Notes = model.Notes
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync(); // ذخیره اولیه برای دریافت شناسه (Id) نوبت

            // واکشی اطلاعات موکل و وکیل برای ارسال پیامک
            var client = await _db.Clients.FindAsync(model.ClientId);
            var lawyer = await _db.Lawyers.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == model.LawyerId);

            if (client != null && lawyer?.User != null)
            {
                // ۱. زمان‌بندی پیامک یادآوری موکل (۲۴ ساعت قبل)
                var appointmentDateTime = latinDate.Add(model.StartTime);
                var reminderTime = appointmentDateTime.AddHours(-24);

                if (reminderTime > DateTime.Now)
                {
                    var clientMessage = $"جناب/سرکار {client.FirstName} {client.LastName}\nیادآوری: نوبت مشاوره شما فردا ساعت {model.StartTime} می‌باشد.";

                    // زمان‌بندی با Hangfire - ارسال تمامی پارامترها برای جلوگیری از خطای Expression Tree
                    var jobId = BackgroundJob.Schedule<ISmsService>(
                        sms => sms.SendAsync(client.PhoneNumber, clientMessage, "AppointmentReminder", appointment.Id),
                        reminderTime - DateTime.Now);
                }

                // ۲. ارسال پیامک فوری به وکیل
                // استفاده از model.AppointmentDate که خودش استرینگ شمسی است
                var lawyerMessage = $"نوبت جدید ثبت شد.\nموکل: {client.FirstName} {client.LastName}\nتاریخ: {model.AppointmentDate}\nساعت: {model.StartTime}";

                // ارسال تمامی پارامترهای اجباری به متد SendAsync
                BackgroundJob.Enqueue<ISmsService>(
                    sms => sms.SendAsync(lawyer.User.PhoneNumber, lawyerMessage, "AppointmentNotification", appointment.Id));
            }

            // تایید تراکنش
            await transaction.CommitAsync();

            return Json(new { success = true, message = "نوبت با موفقیت ثبت شد", id = appointment.Id });
        }
        catch (Exception ex)
        {
            // بازگردانی تراکنش در صورت بروز هرگونه خطا
            await transaction.RollbackAsync();
            return StatusCode(500, new { success = false, message = "خطای سرور در ثبت نوبت" });
        }
    }

    // متد ویرایش نوبت (بررسی هم‌پوشانی انجام می‌شود)
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AppointmentViewModel model)
    {
        if (model == null || model.Id <= 0)
        {
            return BadRequest(new { success = false, message = "اطلاعات نامعتبر است." });
        }

        try
        {
            var appointment = await _db.Appointments.FindAsync(model.Id);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "نوبت یافت نشد." });
            }

 DateTime parsedDate = model.AppointmentDate.ConvertToEnglishNumber().ToLatinDate();

// ۲. سپس متغیر تبدیل شده را در کوئری استفاده کنید
bool hasConflict = await _db.Appointments.AnyAsync(a =>
    a.Id != model.Id && // مستثنی کردن نوبت فعلی در حال ویرایش
    a.LawyerId == model.LawyerId &&
    a.AppointmentDate.Date == parsedDate.Date && // استفاده از متغیر ارزیابی شده
    a.StartTime == model.StartTime &&
    a.Status != AppointmentStatus.Cancelled);

            if (hasConflict)
            {
                return BadRequest(new { success = false, message = "این زمان قبلاً برای این وکیل رزرو شده است." });
            }

            // اعمال تغییرات
            appointment.LawyerId = model.LawyerId;
            appointment.ClientId = model.ClientId;
            appointment.AppointmentDate = model.AppointmentDate.ConvertToEnglishNumber().ToLatinDate();
            appointment.StartTime = model.StartTime;
            appointment.Notes = model.Notes;

            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "زمان ملاقات و اطلاعات نوبت با موفقیت ویرایش شد." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "خطا در ویرایش نوبت." });
        }
    }

    // متد حذف فیزیکی نوبت
    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "نوبت یافت نشد." });
            }

            _db.Appointments.Remove(appointment);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "نوبت با موفقیت حذف شد." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "خطا در حذف نوبت." });
        }
    }


}

public class ChangeStatusRequest
{
    public AppointmentStatus Status { get; set; }
}
