namespace LawOffice.Data.Entities
{
    public class CaseDocument : BaseEntity
    {
        public long CaseId { get; set; }
        public string FileName { get; set; } = string.Empty;       // نام اصلی فایل
        public string StoredFileName { get; set; } = string.Empty; // نام ذخیره شده
        public string RelativePath { get; set; } = string.Empty;   // مسیر نسبی
        public string? Description { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public long UploadedBy { get; set; }

        // Navigation
        public Case Case { get; set; } = null!;
    }
}
