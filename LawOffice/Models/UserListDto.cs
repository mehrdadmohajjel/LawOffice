// مسیر پیشنهادی: Models/DTOs/UserDtos.cs
using System.ComponentModel.DataAnnotations;
using LawOffice.Data.Entities;

namespace LawOffice.Models
{
    public class UserListDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserFormDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string Username { get; set; }

        // در حالت ویرایش می‌تواند خالی باشد
        public string? Password { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "نقش کاربر الزامی است")]
        public UserRole Role { get; set; }
    }
}
