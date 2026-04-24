namespace LawOffice.Data.Entities
{
    public class Lawyer : BaseEntity
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? Level { get; set; }
        public string? BarNumber { get; set; }       // شماره پروانه وکالت
        public string? Specialization { get; set; }  // تخصص
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<Case> Cases { get; set; } = new List<Case>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<CourtSession> CourtSessions { get; set; } = new List<CourtSession>();
    }
}
