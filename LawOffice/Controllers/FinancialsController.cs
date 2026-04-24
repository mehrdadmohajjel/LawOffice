using LawOffice.Data;
using LawOffice.Data.Entities;
using LawOffice.Models; // یا LawOffice.Models.Financial اگر PaymentViewModel در پوشه مجزاست
using LawOffice.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LawOffice.Controllers
{
    [Authorize(Roles = "Staff,Lawyer")]
    public class FinancialsController : Controller
    {
        private readonly AppDbContext _db;

        public FinancialsController(AppDbContext db) => _db = db;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll(long? clientId, long? caseId)
        {
            var query = _db.Financials
                .Include(f => f.Client)
                .Include(f => f.Case)
                .Where(f => !f.IsDeleted) // عدم نمایش موارد حذف شده
                .AsQueryable();

            if (clientId.HasValue) query = query.Where(f => f.ClientId == clientId);
            if (caseId.HasValue) query = query.Where(f => f.CaseId == caseId);

            var list = await query.OrderByDescending(f => f.TransactionDate)
                .Select(f => new
                {
                    f.Id,
                    ClientName = f.Client != null ? f.Client.FirstName + " " + f.Client.LastName : "-",
                    CaseCode = f.Case != null ? f.Case.CaseCode : "-",
                    Type = f.Type.ToString(),
                    f.Amount,
                    f.Description,
                    PaymentMethod = f.PaymentMethod.ToString(),
                    // در صورت داشتن هلپر تاریخ شمسی، اینجا استفاده کنید
                    TransactionDate = f.TransactionDate.ToShamsiDateString("/"),
                    f.ReferenceNumber,
                    // اطلاعات مربوط به چک
                    BankName = f.BankName,
                    DueDate = f.DueDate.HasValue ? f.DueDate.Value.ToShamsiDateString("/") : "",
                    ChequeStatus = f.ChequeStatus.HasValue ? f.ChequeStatus.ToString() : null
                }).ToListAsync();

            var totalIncome = await query
                .Where(f => f.Type == TransactionType.Income)
                .SumAsync(f => f.Amount);

            var totalExpense = await query
                .Where(f => f.Type == TransactionType.Expense)
                .SumAsync(f => f.Amount);

            return Json(new
            {
                success = true,
                data = list,
                summary = new
                {
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    Balance = totalIncome - totalExpense
                }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Lawyer")] // اضافه شدن دسترسی وکیل برای ثبت پرداختی
        public async Task<IActionResult> AddPayment([FromBody] PaymentViewModel model)
        {
            try
            {

           
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "اطلاعات نامعتبر است" });

            var financial = new Financial
            {
                ClientId = model.ClientId,
                CaseId = model.CaseId,
                Type = model.Type,
                Amount = model.Amount,
                Description = model.Description,
                PaymentMethod = model.PaymentMethod,
                TransactionDate = model.TransactionDateShamsi.ConvertToEnglishNumber().ToLatinDate(),
                ReferenceNumber = model.ReferenceNumber,

                // مقداردهی فیلدهای چک در صورت انتخاب روش پرداخت "چک"
                BankName = model.PaymentMethod == PaymentMethod.Cheque ? model.BankName : null,
                DueDate = model.PaymentMethod == PaymentMethod.Cheque ? model.DueDateShamsi.ConvertToEnglishNumber().ToLatinDate() : null,
                ChequeStatus = model.PaymentMethod == PaymentMethod.Cheque ? ChequeStatus.Pending : null,
                SmsReminderSent = false // پیش‌فرض برای سرویس پس‌زمینه
            };

            _db.Financials.Add(financial);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "تراکنش با موفقیت ثبت شد", id = financial.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Lawyer")]
        public async Task<IActionResult> ChangeChequeStatus(long id, [FromBody] ChequeStatus newStatus)
        {
            var financial = await _db.Financials.FindAsync(id);
            if (financial == null || financial.IsDeleted || financial.PaymentMethod != PaymentMethod.Cheque)
                return Json(new { success = false, message = "چک یافت نشد یا تراکنش از نوع چک نیست" });

            financial.ChequeStatus = newStatus;
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "وضعیت چک با موفقیت به‌روزرسانی شد" });
        }

        [HttpDelete]
        [Authorize(Roles = "Staff")] // معمولاً حذف فقط بر عهده کارمند/مدیر است
        public async Task<IActionResult> Delete(long id)
        {
            var financial = await _db.Financials.FindAsync(id);
            if (financial == null || financial.IsDeleted)
                return Json(new { success = false, message = "تراکنش یافت نشد" });

            // حذف منطقی (Soft Delete)
            financial.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "تراکنش با موفقیت حذف شد" });
        }
    }
}
