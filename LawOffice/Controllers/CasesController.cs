// Controllers/CasesController.cs
using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models;
using LawOffice.Services;
using LawOffice.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Admin,Staff,Lawyer")]
    public class CasesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ICaseCodeService _caseCodeService;
        private readonly IFileService _fileService;

        public CasesController(AppDbContext db, ICaseCodeService caseCodeService, IFileService fileService)
        {
            _db = db;
            _caseCodeService = caseCodeService;
            _fileService = fileService;
        }

        public IActionResult Index() => View();
        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _db.CaseStatuses
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    persianTitle = s.PersianTitle
                })
                .ToListAsync();

            return Ok(statuses); // یا (Json(statuses با توجه به ساختار پروژه شما
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cases = await _db.Cases
                .Include(c => c.Client)
                .Include(c => c.Lawyer)
                .Include(c => c.CaseType) // اضافه شدن Include برای دریافت اطلاعات تخصص
                .Include(c => c.CaseStatus)

                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var result = cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Title = c.Title,
                CaseCode = c.CourtCaseNumber,
                CaseTypeId = c.CaseTypeId,
                CaseTypeTitle = c.CaseType?.PersianTitle ?? "نامشخص", // خواندن عنوان فارسی
                CaseStatusId = c.CaseStatusId,
                CaseStatusTitle = c.CaseStatus.PersianTitle,
                ClientId = c.ClientId,
                ClientName = c.Client != null ? $"{c.Client.FirstName} {c.Client.LastName}" : string.Empty,
                LawyerId = c.LawyerId,
                LawyerName = c.Lawyer != null ? $"{c.Lawyer.FirstName} {c.Lawyer.LastName}" : string.Empty,
                CreatedAt = c.CreatedAt,
                TotalFee = c.TotalFee,
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var c = await _db.Cases
                .Include(x => x.CaseType)
                .Include(x => x.Client)
                .Include(x => x.Lawyer)
                .Include(x => x.Notes)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (c == null) return NotFound();

            var model = new CaseViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CaseCode = c.CaseCode,
                CaseTypeId = c.CaseTypeId,
                CaseTypeTitle = c.CaseType?.PersianTitle,
                TotalFee = c.TotalFee,
                ClientId = c.ClientId,
                ClientFirstName = c.Client.FirstName,
                ClientLastName = c.Client.LastName,
                ClientName = c.Client.FirstName + " " + c.Client.LastName,

                LawyerId = c.LawyerId,
                LawyerFirstName = c.Lawyer.FirstName,
                LawyerLastName = c.Lawyer.LastName,
                LawyerName = c.Lawyer.FirstName + " " + c.Lawyer.LastName,

                CaseStatusId = c.CaseStatusId,
                CourtBranch = c.CourtBranch,
                CourtCaseNumber = c.CourtCaseNumber,
                CourtName = c.CourtName,
                OpenDate = c.OpenDate.HasValue ? c.OpenDate.Value.ToShamsiDateString() : "",
                OpponentName = c.OpponentName,
                OpponentLawyer = c.OpponentLawyer,
                CaseNotes = c.Notes.Select(n => new CaseNoteViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    // تبدیل تاریخ میلادی به شمسی برای نمایش در فرانت‌اِند
                    CreatedAt = n.CreatedAt.ToShamsiDateString()
                }).ToList()

            };

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CaseViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("اطلاعات معتبر نیست.");

            // تولید کد پرونده با استفاده از شناسه تخصص
            var caseCode = await _caseCodeService.GenerateAsync(model.CaseTypeId);

            var caseEntity = new Case
            {
                ClientId = model.ClientId,
                LawyerId = model.LawyerId,
                Title = model.Title,
                Description = model.Description,
                CaseCode = caseCode,
                OpponentName = model.OpponentName,
                OpponentLawyer = model.OpponentLawyer,
                CourtBranch = model.CourtBranch,
                CourtCaseNumber = model.CourtCaseNumber,
                CourtName = model.CourtName,
                OpenDate = !string.IsNullOrWhiteSpace(model.OpenDate)
        ? model.OpenDate.ConvertToEnglishNumber().ToLatinDate()
        : null,
                CaseTypeId = model.CaseTypeId, // استفاده از شناسه جدول
                CaseStatusId = model.CaseStatusId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Cases.Add(caseEntity);
            await _db.SaveChangesAsync();

            return Ok(new { message = "پرونده با موفقیت ایجاد شد.", caseId = caseEntity.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CaseViewModel model)
        {
            try
            {


                var caseEntity = await _db.Cases.FindAsync(model.Id);
                if (model.CaseTypeId != caseEntity.CaseTypeId)
                {
                    caseEntity.CaseCode = await _caseCodeService.GenerateAsync(model.CaseTypeId);
                }
                else
                {
                    caseEntity.CaseCode = model.CaseCode;

                }
                if (caseEntity == null) return NotFound();

                caseEntity.Title = model.Title;
                caseEntity.Description = model.Description;
                caseEntity.ClientId = model.ClientId;
                caseEntity.LawyerId = model.LawyerId;
                caseEntity.CaseTypeId = model.CaseTypeId; // بروزرسانی نوع پرونده
                caseEntity.CaseStatusId = model.CaseStatusId;
                caseEntity.CourtBranch = model.CourtBranch;
                caseEntity.CourtCaseNumber = model.CourtCaseNumber;
                caseEntity.CourtName = model.CourtName;
                caseEntity.TotalFee = model.TotalFee;
                caseEntity.OpenDate = !string.IsNullOrWhiteSpace(model.OpenDate)
            ? model.OpenDate.ConvertToEnglishNumber().ToLatinDate() : null;
                await _db.SaveChangesAsync();
                return Ok(new { message = "پرونده به‌روزرسانی شد." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var caseEntity = await _db.Cases.FindAsync(id);

            if (caseEntity == null || caseEntity.IsDeleted)
                return NotFound("پرونده یافت نشد.");

            caseEntity.IsDeleted = true;
            caseEntity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok("پرونده با موفقیت حذف شد.");
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] CaseNoteViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("اطلاعات یادداشت معتبر نیست.");

            var caseEntity = await _db.Cases.FindAsync(model.CaseId);

            if (caseEntity == null || caseEntity.IsDeleted)
                return NotFound("پرونده یافت نشد.");

            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = new CaseNote
            {
                CaseId = model.CaseId,
                Content = model.Content,
                AuthorId = userId,  // ✅ AuthorId نه CreatedBy
                CreatedAt = DateTime.UtcNow
            };

            _db.Set<CaseNote>().Add(note); // ✅ اگر DbSet مستقیم نداری
            await _db.SaveChangesAsync();

            return Ok("یادداشت با موفقیت اضافه شد.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(long id)
        {
            var note = await _db.Set<CaseNote>().FindAsync(id);

            if (note == null || note.IsDeleted)
                return NotFound("یادداشت یافت نشد.");

            note.IsDeleted = true;
            note.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok("یادداشت با موفقیت حذف شد.");
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentDto model)
        {
            try
            {
                // دسترسی به متغیرها از داخل مدل
                long caseId = model.CaseId;
                IFormFile file = model.File;
                string description = model.Description;

                if (caseId <= 0)
                    return BadRequest(new { message = "شناسه پرونده نامعتبر است." });

                var caseEntity = await _db.Cases.FindAsync(caseId);
                if (caseEntity == null) return NotFound(new { message = "پرونده یافت نشد." });

                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "لطفاً یک فایل معتبر انتخاب کنید." });

                // ادامه کدهای قبلی برای ذخیره فایل...
                string folderName = !string.IsNullOrWhiteSpace(caseEntity.CaseCode) ? caseEntity.CaseCode : $"Case_{caseId}";

                var (storedName, relativePath) = await _fileService.SaveAsync(file, folderName);

                long userId = 1;
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
                }

                var document = new CaseDocument
                {
                    CaseId = caseId,
                    FileName = file.FileName,
                    StoredFileName = storedName,
                    RelativePath = relativePath,
                    Description = description,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    UploadedBy = userId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _db.CaseDocuments.Add(document);
                await _db.SaveChangesAsync();

                return Ok(new { message = "فایل با موفقیت آپلود شد." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetDocuments(long caseId)
        {
            // ۱. ابتدا داده‌های مورد نیاز را از دیتابیس دریافت می‌کنیم (ترجمه به SQL)
            var rawDocs = await _db.CaseDocuments
                .Where(d => d.CaseId == caseId && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new
                {
                    d.Id,
                    d.FileName,
                    d.RelativePath,
                    d.Description,
                    d.FileSize,
                    d.CreatedAt
                })
                .ToListAsync(); // در اینجا کوئری روی دیتابیس اجرا می‌شود

            // ۲. حالا تبدیل تاریخ و محاسبات را روی داده‌های دریافت شده (در حافظه) انجام می‌دهیم
            var docs = rawDocs.Select(d => new
            {
                d.Id,
                d.FileName,
                d.RelativePath,
                d.Description,
                FileSize = Math.Round((double)d.FileSize / 1024, 2),
                CreatedAt = d.CreatedAt.ToShamsiDateString()
            }).ToList();

            return Ok(new { data = docs });
        }
        [HttpGet]
        public IActionResult Details(long id)
        {
            // ارسال شناسه پرونده به View
            ViewBag.CaseId = id;
            return View("Detail");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument(long id)
        {
            var document = await _db.Set<CaseDocument>().FindAsync(id);

            if (document == null || document.IsDeleted)
                return NotFound("مدرک یافت نشد.");

            if (!_fileService.Exists(document.RelativePath))
                return NotFound("فایل یافت نشد.");

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                document.RelativePath.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(fileBytes, document.ContentType, document.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(long id)
        {
            var doc = await _db.CaseDocuments.FindAsync(id);
            if (doc == null || doc.IsDeleted) return NotFound(new { message = "فایل یافت نشد." });

            // حذف فیزیکی فایل
            if (_fileService.Exists(doc.RelativePath))
            {
                _fileService.Delete(doc.RelativePath);
            }

            // حذف منطقی (Soft Delete) از دیتابیس
            doc.IsDeleted = true;
            doc.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();

            return Ok(new { message = "فایل با موفقیت حذف شد." });
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseTypes()
        {
            var types = await _db.CaseTypes
                .Select(t => new { t.Id, t.PersianTitle, t.Perfix })
                .ToListAsync();
            return Ok(types);
        }
        [HttpPost]
        public async Task<IActionResult> SaveNote([FromBody] CaseNoteViewModel model)
        {
            try
            {
                if (model.CaseId <= 0 || string.IsNullOrWhiteSpace(model.Content))
                    return Json(new { success = false, message = "داده‌های ورودی نامعتبر است." });

                if (model.Id == 0)
                {
                    // ثبت یادداشت جدید
                    var note = new CaseNote
                    {
                        CaseId = model.CaseId,
                        Title = model.Title,
                        Content = model.Content,
                        CreatedAt = DateTime.Now,
                        // در صورت وجود سیستم احراز هویت، شناسه کاربر لاگین شده را نیز ثبت کنید
                        AuthorId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    };
                    _db.CaseNotes.Add(note);
                }
                else
                {
                    // ویرایش یادداشت (در صورت نیاز)
                    var note = await _db.CaseNotes.FindAsync(model.Id);
                    if (note != null)
                    {
                        note.Title = model.Title;
                        note.Content = model.Content;
                    }
                }

                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا در ذخیره یادداشت: " + ex.Message });
            }
        }

        [HttpGet("PrintContract/{id}")]
        public async Task<IActionResult> PrintContract(long id)
        {
            var caseItem = await _db.Cases
                .Include(c => c.Client)
                .Include(c => c.Lawyer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (caseItem == null)
                return NotFound("پرونده یافت نشد");

            var model = new ContractPrintViewModel
            {
                // وکیل
                LawyerName = caseItem.Lawyer.FirstName,
                LawyerLastName = caseItem.Lawyer.LastName,
                LawyerFatherName = caseItem.Lawyer.FatherName,
                LawyerNationalCode = caseItem.Lawyer.NationalCode,
                LawyerLicenseNumber = caseItem.Lawyer.BarNumber,
                LawyerLevel = caseItem.Lawyer.Level,

                // موکل
                ClientName = caseItem.Client.FirstName,
                ClientLastName = caseItem.Client.LastName,
                ClientFatherName = caseItem.Client.FatherName,
                ClientNationalCode = caseItem.Client.NationalCode,
                ClientAddress = caseItem.Client.Address,
                ClientPhone = caseItem.Client.PhoneNumber,

                // قرارداد
                ContractDate = DateTime.Now.ToString("yyyy/MM/dd"),
                ContractNumber = DateTime.Now.Ticks.ToString(),
                FeeAmount = "0 ریال"  // می‌تونید از Financial بردارید
            };

            return View(model);
        }

    }
}
