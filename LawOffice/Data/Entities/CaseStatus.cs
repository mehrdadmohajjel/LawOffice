using LawOffice.Data.Entities;

//public enum CaseStatus
//{
//    Active = 1,     // فعال
//    Pending = 2,    // در انتظار
//    Closed = 3,     // بسته شده
//    Archived = 4    // آرشیو
//}
public class CaseStatus : BaseEntity 
{
    public string Name { get; set; } 
    public string PersianTitle { get; set; }
    public int DisplayOrder { get; set; }
    public virtual ICollection<Case> Cases { get; set; }
}