namespace LawOffice.Data.Entities
{
    public class CaseNote : BaseEntity
    {
        public long CaseId { get; set; }
        public long AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPrivate { get; set; } = false; // فقط وکیل ببیند

        // Navigation
        public Case Case { get; set; } = null!;
        public User Author { get; set; } = null!;
    }
}
