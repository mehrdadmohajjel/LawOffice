namespace LawOffice.Data.Entities
{
    //public enum CaseType
    //{
    //    Civil = 1,          // حقوقی
    //    Criminal = 2,       // کیفری
    //    Family = 3,         // خانواده
    //    Labor = 4,          // کار
    //    Commercial = 5,     // تجاری
    //    Administrative = 6, // اداری
    //    Real_Estate = 7,    // ملکی
    //    Other = 8           // سایر
    //}
    public class CaseType : BaseEntity // در صورت وجود BaseEntity، ارث‌بری کنید
    {
        public string Name { get; set; } // نام انگلیسی (مثلا Civil)
        public string PersianTitle { get; set; } // عنوان فارسی برای نمایش (مثلا حقوقی)
        public string Perfix { get; set; }


        public virtual ICollection<Case> Cases { get; set; }
    }
}
