namespace LawOffice.Data.Entities
{
    public class SmsLog : BaseEntity
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsSent { get; set; }
        public string? ErrorMessage { get; set; }
        public string ReferenceType { get; set; } = string.Empty; // Appointment / CourtSession
        public long ReferenceId { get; set; }
    }
}
