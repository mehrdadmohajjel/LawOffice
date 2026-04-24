namespace LawOffice.Data.Entities
{
    public class Appointment : BaseEntity
    {
        public long LawyerId { get; set; }
        public long? ClientId { get; set; }
        public long? CaseId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Available;
        public string? Notes { get; set; }
        public bool ReminderSent1Day { get; set; } = false;
        public bool ReminderSent3Days { get; set; } = false;
        public string? ReminderJobId { get; set; }

        // Navigation
        public Lawyer Lawyer { get; set; } = null!;
        public Client? Client { get; set; }
        public Case? Case { get; set; }
    }
}
