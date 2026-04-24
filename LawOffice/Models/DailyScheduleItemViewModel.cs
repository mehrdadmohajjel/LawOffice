namespace LawOffice.Models
{
    public class DailyScheduleItemViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; } // "Appointment" یا "CourtSession"
        public string Time { get; set; }
        public string Title { get; set; } // عنوان نوبت یا نام دادگاه
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string LawyerName { get; set; }
        public int Status { get; set; }
    }
}
