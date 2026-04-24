namespace LawOffice.Data.Entities
{
    public class CourtSession : BaseEntity
    {
        public long CaseId { get; set; }
        public long LawyerId { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public string CourtName { get; set; }
        public string? Branch { get; set; }
        public string? Judge { get; set; }
        public CourtSessionStatus Status { get; set; } = CourtSessionStatus.Scheduled;
        public string? Result { get; set; }
        public string? Notes { get; set; }
        public bool ReminderSent1Day { get; set; }
        public bool ReminderSent3Days { get; set; }

        // Navigation Properties
        public Case Case { get; set; }
        public Lawyer Lawyer { get; set; }
    }
}
