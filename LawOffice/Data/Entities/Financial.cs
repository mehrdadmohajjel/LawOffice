// فایل: Data/Entities/Financial.cs
using System;

namespace LawOffice.Data.Entities
{
    public class Financial : BaseEntity
    {
        public long? ClientId { get; set; }
        public long? CaseId { get; set; }
        public TransactionType Type { get; set; } // همیشه Income برای حق‌الوکاله
        public decimal Amount { get; set; }
        public string? Description { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; } // تاریخ پرداخت نقدی یا تاریخ ثبت چک

        // --- فیلدهای جدید برای چک ---
        public DateTime? DueDate { get; set; } // تاریخ سررسید چک (یا قسط)
        public string? BankName { get; set; } // نام بانک
        public string? ReferenceNumber { get; set; } // شماره چک یا شماره پیگیری فیش
        public ChequeStatus? ChequeStatus { get; set; } // وضعیت چک
        public bool SmsReminderSent { get; set; } = false; // بررسی ارسال پیامک یادآوری
        // -----------------------------

        public long? RegisteredBy { get; set; }

        // Navigation
        public Client? Client { get; set; }
        public Case? Case { get; set; }
    }
}
