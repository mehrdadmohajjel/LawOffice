// Data/Entities/Case.cs
namespace LawOffice.Data.Entities
{
    public class Case : BaseEntity
    {
        public string CaseCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public long CaseTypeId { get; set; }
        public virtual CaseType CaseType { get; set; }
        public long CaseStatusId { get; set; }
        public virtual CaseStatus CaseStatus { get; set; }
        public long ClientId { get; set; }
        public long LawyerId { get; set; }
        public string? CourtName { get; set; }
        public string? CourtBranch { get; set; }
        public string? CourtCaseNumber { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string? Description { get; set; }
        public string? Result { get; set; }
        public decimal? TotalFee { get; set; }

        // ✅ فیلدهای اضافه‌شده
        public string? OpponentName { get; set; }
        public string? OpponentLawyer { get; set; }

        // Navigation
        public Client Client { get; set; } = null!;
        public Lawyer Lawyer { get; set; } = null!;
        public ICollection<CaseDocument> Documents { get; set; } = new List<CaseDocument>();
        public ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();
        public ICollection<CourtSession> CourtSessions { get; set; } = new List<CourtSession>();
        public ICollection<Financial> Financials { get; set; } = new List<Financial>();
    }
}
