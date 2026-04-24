using LawOffice.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class CreateClientDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        [Display(Name = "نام")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "کد ملی الزامی است")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید ۱۰ رقم باشد")]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; } = string.Empty;

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "شماره تماس")]
        [Phone(ErrorMessage = "شماره تماس معتبر نیست")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string? Email { get; set; }

        [Display(Name = "تحصیلات")]
        public EducationLevel? Education { get; set; }

        [Display(Name = "یادداشت")]
        public string? Notes { get; set; }

        // برای ایجاد حساب کاربری
        [Display(Name = "نام کاربری")]
        public string? Username { get; set; }

        [Display(Name = "رمز عبور")]
        public string? Password { get; set; }
    }

}
