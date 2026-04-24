using LawOffice.Data;
using LawOffice.Utilities;
using Microsoft.EntityFrameworkCore;

namespace LawOffice.Services
{
    public interface ICaseCodeService
    {
        // بهتر است شناسه نوع پرونده را به عنوان ورودی بگیریم
        Task<string> GenerateAsync(long caseTypeId);
    }

    public class CaseCodeService : ICaseCodeService
    {
        private readonly AppDbContext _db;

        public CaseCodeService(AppDbContext db) => _db = db;

        public async Task<string> GenerateAsync(long caseTypeId)
        {

            try
            {


                var caseType = await _db.CaseTypes.FindAsync(caseTypeId);
                if (caseType == null)
                {
                    throw new ArgumentException("نوع پرونده یافت نشد.");
                }

                var prefix = caseType.Perfix;

                string year = DateTime.Now.GetShamsiYear();

                var count = await _db.Cases
                    .CountAsync(c => c.CaseTypeId == caseTypeId) + 1;

                return $"{prefix}-{year}-{count:D5}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
