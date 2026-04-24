using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Admin,Staff")] // سطح دسترسی دلخواه
    public class LawyersController : Controller
    {
        private readonly AppDbContext _db;

        public LawyersController(AppDbContext db)
        {
            _db = db;
        }

        // --- باز کردن صفحات ---
        public IActionResult Index() => View();

        public async Task<IActionResult> Details(long id)
        {
            var lawyer = await _db.Lawyers.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);
            if (lawyer == null) return NotFound();
            return View(lawyer);
        }

        // --- API ها برای استفاده در جاوااسکریپت ---

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lawyers = await _db.Lawyers
                .Select(l => new {
                    l.Id,
                    l.FirstName,
                    l.LastName,
                    l.BarNumber,
                    l.Specialization,
                    l.PhoneNumber,
                    l.NationalCode,
                    l.Level,
                    l.FatherName
                })
                .OrderByDescending(l => l.Id)
                .ToListAsync();

            return Json(new { data = lawyers });
        }

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var lawyer = await _db.Lawyers.FindAsync(id);
            if (lawyer == null) return Json(new { success = false, message = "وکیل یافت نشد" });
            return Json(new { success = true, data = lawyer });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LawyerViewModel model)
        {
            if (model == null) return Json(new { success = false, message = "اطلاعات نامعتبر است" });

            var lawyer = new Lawyer
            {
                UserId = model.UserId, // باید از سمت کلاینت ارسال شود (مثلا سلکت باکس کاربران)
                FirstName = model.FirstName,
                LastName = model.LastName,
                BarNumber = model.BarNumber,
                Specialization = model.Specialization,
                PhoneNumber = model.PhoneNumber,
                Bio = model.Bio,
                NationalCode = model.NationalCode,
                FatherName= model.FatherName,
                Level = model.Level

            };

            _db.Lawyers.Add(lawyer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "وکیل با موفقیت ثبت شد" });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] LawyerViewModel model)
        {
            if (model == null) return Json(new { success = false, message = "اطلاعات نامعتبر است" });

            var lawyer = await _db.Lawyers.FindAsync(model.Id);
            if (lawyer == null) return Json(new { success = false, message = "وکیل یافت نشد" });

            lawyer.FirstName = model.FirstName;
            lawyer.LastName = model.LastName;
            lawyer.BarNumber = model.BarNumber;
            lawyer.Specialization = model.Specialization;
            lawyer.PhoneNumber = model.PhoneNumber;
            lawyer.Bio = model.Bio;
            lawyer.FatherName = model.FatherName;
            lawyer.Level = model.Level; 
            lawyer.NationalCode = model.NationalCode;

            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "اطلاعات وکیل بروزرسانی شد" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var lawyer = await _db.Lawyers.FindAsync(id);
            if (lawyer == null) return Json(new { success = false, message = "وکیل یافت نشد" });

            _db.Lawyers.Remove(lawyer);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "وکیل با موفقیت حذف شد" });
        }
    }
}
