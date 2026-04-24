using LawOffice.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class AppointmentViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "وکیل الزامی است")]
        [Display(Name = "وکیل")]
        public long LawyerId { get; set; }

        [Display(Name = "موکل")]
        public long? ClientId { get; set; }

        [Display(Name = "پرونده")]
        public long? CaseId { get; set; }

        [Required(ErrorMessage = "تاریخ الزامی است")]
        [Display(Name = "تاریخ")]
        public string AppointmentDate { get; set; }

        [Required(ErrorMessage = "ساعت شروع الزامی است")]
        [Display(Name = "ساعت شروع")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "ساعت پایان الزامی است")]
        [Display(Name = "ساعت پایان")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "وضعیت")]
        public AppointmentStatus Status { get; set; }

        [Display(Name = "یادداشت")]
        public string? Notes { get; set; }

        // برای نمایش
        public string? LawyerName { get; set; }
        public string? ClientName { get; set; }
        public string? CaseCode { get; set; }
    }

    public class TimeSlotModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public long? AppointmentId { get; set; }
        public string? ClientName { get; set; }
    }

}
