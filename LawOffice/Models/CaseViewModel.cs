using LawOffice.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class CaseViewModel
    {
        public long Id { get; set; }

        [Display(Name = "کد پرونده")]
        public string CaseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "عنوان پرونده الزامی است")]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع پرونده الزامی است")]
        [Display(Name = "نوع پرونده")]
        public long CaseTypeId { get; set; }
        public string? CaseTypeTitle { get; set; } // برای نمایش نام تخصص در جداول

        [Required(ErrorMessage = "وضعیت پرونده الزامی است")]
        [Display(Name = "وضعیت")]
        public long CaseStatusId { get; set; }

        public string? CaseStatusTitle { get; set; } // برای نمایش نام وضعیت در جداول

        [Required(ErrorMessage = "موکل الزامی است")]
        [Display(Name = "موکل")]
        public long ClientId { get; set; }

        [Required(ErrorMessage = "وکیل الزامی است")]
        [Display(Name = "وکیل")]
        public long LawyerId { get; set; }

        [Display(Name = "نام دادگاه")]
        public string? CourtName { get; set; }

        [Display(Name = "شعبه")]
        public string? CourtBranch { get; set; }

        [Display(Name = "شماره پرونده دادگاه")]
        public string? CourtCaseNumber { get; set; }

        [Display(Name = "تاریخ افتتاح")]
        public string? OpenDate { get; set; }

        [Display(Name = "تاریخ بسته شدن")]
        public string? CloseDate { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "نتیجه")]
        public string? Result { get; set; }

        // ✅ فیلدهای اضافه‌شده برای طرف مقابل
        [Display(Name = "نام طرف مقابل")]
        public string? OpponentName { get; set; }

        [Display(Name = "وکیل طرف مقابل")]
        public string? OpponentLawyer { get; set; }

        // ✅ فیلدهای نمایشی موکل
        [Display(Name = "نام موکل")]
        public string? ClientFirstName { get; set; }

        [Display(Name = "نام خانوادگی موکل")]
        public string? ClientLastName { get; set; }

        [Display(Name = "نام کامل موکل")]
        public string? ClientName { get; set; }

        [Display(Name = "کد ملی موکل")]
        public string? ClientNationalCode { get; set; }

        [Display(Name = "شماره تماس موکل")]
        public string? ClientPhoneNumber { get; set; }

        [Display(Name = "آدرس موکل")]
        public string? ClientAddress { get; set; }

        // ✅ فیلدهای نمایشی وکیل
        [Display(Name = "نام وکیل")]
        public string? LawyerFirstName { get; set; }

        [Display(Name = "نام خانوادگی وکیل")]
        public string? LawyerLastName { get; set; }

        [Display(Name = "نام کامل وکیل")]
        public string? LawyerName { get; set; }

        // ✅ فیلدهای اضافی
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedAt { get; set; }

        public bool HasCourtSessions { get; set; }
        [Display(Name = "حق الوکاله")]
        public decimal? TotalFee { get; set; }

        // ✅ لیست یادداشت‌ها و مدارک
        public List<CaseNoteViewModel> CaseNotes { get; set; } = new();
        public List<CaseDocumentViewModel> CaseDocuments { get; set; } = new();
    }

    // ✅ CaseNoteViewModel
    public class CaseNoteViewModel
    {
        public long Id { get; set; }
        public long CaseId { get; set; }

        [Required(ErrorMessage = "متن یادداشت الزامی است")]
        [Display(Name = "یادداشت")]
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    // ✅ CaseDocumentViewModel
    public class CaseDocumentViewModel
    {
        public long Id { get; set; }
        public long CaseId { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; } = string.Empty;

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
    }
}
