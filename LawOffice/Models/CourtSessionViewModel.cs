using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class CourtSessionViewModel
    {
        public long Id { get; set; }
        public long CaseId { get; set; }
        public long LawyerId { get; set; }

        [Required(ErrorMessage = "تاریخ جلسه الزامی است")]
        public string SessionDate { get; set; } // تاریخ شمسی (مثلاً 1405/01/31)

        [Required(ErrorMessage = "ساعت جلسه الزامی است")]
        public string SessionTime { get; set; } // ساعت (مثلاً 10:30)

        [Required(ErrorMessage = "نام مجتمع قضایی الزامی است")]
        public string CourtName { get; set; }

        public string? Branch { get; set; }
        public string? Judge { get; set; }
        public string? Notes { get; set; }
    }

}
