using LawOffice.Data;
using LawOffice.Models;
using LawOffice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawOffice.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext db, IJwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new LoginResultModel
                {
                    Success = false,
                    Message = "اطلاعات ورودی نامعتبر است"
                });

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Json(new LoginResultModel
                {
                    Success = false,
                    Message = "نام کاربری یا رمز عبور اشتباه است"
                });

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Json(new LoginResultModel
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new UserInfoModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Role = user.Role.ToString(),
                    RoleId = (int)user.Role
                }
            });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken
                    && u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return Unauthorized();

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            Response.Cookies.Append("access_token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Json(new { success = true, accessToken = newAccessToken });
        }
    }

}
