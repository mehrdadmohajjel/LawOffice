using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LawOffice.Utilities;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class ClientsController : Controller
    {
        private readonly AppDbContext _db;

        public ClientsController(AppDbContext db) => _db = db;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // مرحله اول: دریافت اطلاعات از دیتابیس (بدون تبدیل شمسی)
            var clientsQuery = await _db.Clients
                .Select(c => new
                {
                    c.Id,
                    c.FirstName,
                    c.LastName,
                    c.NationalCode,
                    c.PhoneNumber,
                    c.Email,
                    c.BirthDate,
                    c.Education,
                    CasesCount = c.Cases.Count
                })
                .ToListAsync();

            // مرحله دوم: تبدیل تاریخ شمسی در حافظه سیستم (جلوگیری از خطای EF Core و CS0854)
            var clients = clientsQuery.Select(c => new
            {
                c.Id,
                c.FirstName,
                c.LastName,
                FullName = c.FirstName + " " + c.LastName,
                c.NationalCode,
                c.PhoneNumber,
                c.Email,
                BirthDate = c.BirthDate.HasValue ? c.BirthDate.Value.ToShamsiDateString() : "",
                Education = c.Education.HasValue ? c.Education.Value.ToString() : "",
                c.CasesCount
            }).ToList();

            return Json(new { success = true, data = clients });
        }
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            // ۱. دریافت اطلاعات موکل از دیتابیس
            var client = await _db.Clients.FindAsync(id);

            // ۲. بررسی وجود موکل
            if (client == null)
            {
                // چون این یک صفحه است (نه درخواست ای‌پی‌آی)، در صورت نبود اطلاعات، صفحه 404 برمی‌گردانیم
                return NotFound("موکل مورد نظر یافت نشد.");
            }

            // ۳. ارسال مستقیم خود شیء موکل به صفحه View
            return View(client);
        }
        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null)
                return Json(new { success = false, message = "موکل یافت نشد" });

            // برگرداندن یک آبجکت سفارشی تا بتوانیم تاریخ شمسی را به صورت متنی (String) ارسال کنیم
            return Json(new
            {
                success = true,
                data = new
                {
                    client.Id,
                    client.FirstName,
                    client.LastName,
                    client.NationalCode,
                    client.Address,
                    client.PhoneNumber,
                    client.Email,
                    Education = client.Education?.ToString(),
                    client.Notes,
                    // تبدیل تاریخ میلادی به شمسی برای فرم ویرایش
                    BirthDate = client.BirthDate.HasValue ? client.BirthDate.Value.ToShamsiDateString() : ""
                }
            });
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                return Json(new { success = false, message = "اطلاعات نامعتبر است" });
            }

            if (await _db.Clients.AnyAsync(c => c.NationalCode == model.NationalCode))
                return Json(new { success = false, message = "کد ملی تکراری است" });

            User? user = null;
            if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password))
            {
                if (await _db.Users.AnyAsync(u => u.Username == model.Username))
                    return Json(new { success = false, message = "نام کاربری تکراری است" });

                user = new User
                {
                    Username = model.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Role = UserRole.Client,
                    IsActive = true
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }


            var client = new Client
            {
                UserId = user?.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NationalCode = model.NationalCode,
                // بررسی ایمن برای جلوگیری از خطای NullReference
                BirthDate = !string.IsNullOrWhiteSpace(model.BirthDate)
        ? model.BirthDate.ConvertToEnglishNumber().ToLatinDate()
        : null,

                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Education = model.Education,
                Notes = model.Notes
            };

            _db.Clients.Add(client);
            await _db.SaveChangesAsync(); // اگر دیتابیس خطا بدهد در این خط رخ می‌دهد


            return Json(new { success = true, message = "موکل با موفقیت ثبت شد", id = client.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ClientViewModel model)
        {
            var client = await _db.Clients.FindAsync(model.Id);
            if (client == null)
                return Json(new { success = false, message = "موکل یافت نشد" });

            if (await _db.Clients.AnyAsync(c => c.NationalCode == model.NationalCode && c.Id != model.Id))
                return Json(new { success = false, message = "کد ملی تکراری است" });

            client.FirstName = model.FirstName;
            client.LastName = model.LastName;
            client.NationalCode = model.NationalCode;
            client.BirthDate = !string.IsNullOrWhiteSpace(model.BirthDate)
                    ? model.BirthDate.ConvertToEnglishNumber().ToLatinDate()
                    : null;
            client.Address = model.Address;
            client.PhoneNumber = model.PhoneNumber;
            client.Email = model.Email;
            client.Education = model.Education;
            client.Notes = model.Notes;
            client.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "موکل با موفقیت ویرایش شد" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null)
                return Json(new { success = false, message = "موکل یافت نشد" });

            client.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "موکل با موفقیت حذف شد" });
        }


    }
}
