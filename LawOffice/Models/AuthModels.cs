using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class LoginResultModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public UserInfoModel? User { get; set; }
    }

    public class UserInfoModel
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
