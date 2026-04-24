namespace LawOffice.Data.Entities
{
    public class Staff : BaseEntity
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; } // سمت (منشی، کارشناس، ...)
        public string? Notes { get; set; }

        // Navigation
        public User User { get; set; } = null!;
    }
}
