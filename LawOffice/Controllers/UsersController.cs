using Humanizer;
using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FirstName + " " + u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Role = u.Role,
                    RoleName = GetRoleName(u.Role),
                    IsActive = u.IsActive
                }).ToListAsync();

            return Json(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "کاربر یافت نشد." });

            var dto = new UserFormDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = user.Role
                // پسورد را برای کلاینت ارسال نمی‌کنیم
            };
            return Json(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] UserFormDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Id == 0) // ایجاد کاربر جدید
            {
                if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                    return BadRequest(new { message = "نام کاربری تکراری است." });

                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { message = "کلمه عبور برای کاربر جدید الزامی است." });

                var newUser = new User
                {
                    Username = request.Username,
                    // نکته امنیتی: در سیستم واقعی حتماً از ابزارهای Hashing مثل BCrypt استفاده کنید
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Role = request.Role,
                    IsActive = true
                };

                _db.Users.Add(newUser);
                await _db.SaveChangesAsync();
                return Ok(new { message = "کاربر با موفقیت ایجاد شد." });
            }
            else // ویرایش کاربر
            {
                var user = await _db.Users.FindAsync(request.Id);
                if (user == null) return NotFound(new { message = "کاربر یافت نشد." });

                // بررسی نام کاربری تکراری برای سایرین
                if (await _db.Users.AnyAsync(u => u.Username == request.Username && u.Id != request.Id))
                    return BadRequest(new { message = "نام کاربری تکراری است." });

                user.Username = request.Username;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                user.Email = request.Email;
                user.Role = request.Role;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                }

                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return Ok(new { message = "اطلاعات کاربر با موفقیت ویرایش شد." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(long id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "کاربر یافت نشد." });

            user.IsActive = !user.IsActive;
            await _db.SaveChangesAsync();

            string status = user.IsActive ? "فعال" : "غیرفعال";
            return Ok(new { message = $"کاربر با موفقیت {status} شد." });
        }

        // متد کمکی برای ترجمه نام نقش
        private static string GetRoleName(UserRole role)
        {
            return role switch
            {
                UserRole.Admin => "مدیر سیستم",
                UserRole.Client => "موکل",
                UserRole.Lawyer => "وکیل",
                UserRole.Staff => "کارمند",
                _ => "نامشخص"
            };
        }
    }
}
