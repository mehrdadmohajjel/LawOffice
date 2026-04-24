using Microsoft.VisualBasic;

namespace LawOffice.Data.Entities
{
    public class Client : BaseEntity
    {
        public long? UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public EducationLevel? Education { get; set; }
        public string? Notes { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<Case> Cases { get; set; } = new List<Case>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Financial> Financials { get; set; } = new List<Financial>();
    }
}
