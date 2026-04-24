using LawOffice.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LawOffice.Models
{
    public class FinancialViewModel
    {
        public long Id { get; set; }

        [Display(Name = "موکل")]
        public long? ClientId { get; set; }

        [Display(Name = "پرونده")]
        public long? CaseId { get; set; }

        [Required(ErrorMessage = "نوع تراکنش الزامی است")]
        [Display(Name = "نوع")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "مبلغ الزامی است")]
        [Display(Name = "مبلغ (ریال)")]
        [Range(1, double.MaxValue, ErrorMessage = "مبلغ باید بیشتر از صفر باشد")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "شرح الزامی است")]
        [Display(Name = "شرح")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "روش پرداخت الزامی است")]
        [Display(Name = "روش پرداخت")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "تاریخ الزامی است")]
        [Display(Name = "تاریخ")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "شماره مرجع")]
        public string? ReferenceNumber { get; set; }

        // برای نمایش
        public string? ClientName { get; set; }
        public string? CaseCode { get; set; }
    }
}
