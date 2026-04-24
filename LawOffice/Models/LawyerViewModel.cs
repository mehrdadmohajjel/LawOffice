namespace LawOffice.Models
{
    public class LawyerViewModel
    {
        public long Id { get; set; }

        // شناسه کاربری برای اتصال وکیل به اکانت سیستم (در صورت لزوم می‌توانید آن را از لیست کشویی در فرم انتخاب کنید)
        public long UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? BarNumber { get; set; }
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? Level { get; set; }
    }
}
