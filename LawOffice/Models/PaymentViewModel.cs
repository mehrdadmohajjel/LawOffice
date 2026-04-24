using LawOffice.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class PaymentViewModel
    {
        public long Id { get; set; }
        [Required]
        public long CaseId { get; set; }
        [Required]
        public long ClientId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public string? Description { get; set; }

        // این تاریخ از سمت کلاینت شمسی می‌آید و باید در کنترلر میلادی شود
        [Required]
        public string TransactionDateShamsi { get; set; }
        public TransactionType Type { get; set; }

        // اطلاعات چک
        public string? DueDateShamsi { get; set; }
        public string? BankName { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
