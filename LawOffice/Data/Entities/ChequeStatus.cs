namespace LawOffice.Data.Entities
{
    public enum ChequeStatus
    {
        Pending = 1,   // در انتظار / وصول نشده
        Cashed = 2,    // پاس شده
        Bounced = 3,   // برگشت خورده
        Cancelled = 4  // لغو شده
    }

}
