using LawOffice.Data;
using LawOffice.Models;
using LawOffice.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
// فضای نام اکستنشن‌های تاریخ پروژه خود را اضافه کنید (برای ToLatinDate)

[Authorize(Roles = "Admin,Staff,Lawyer")]
public class DailyScheduleController : Controller
{
    private readonly AppDbContext _db;

    public DailyScheduleController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetDailySchedule(string shamsiDate)
    {
        // تبدیل تاریخ شمسی به میلادی (با استفاده از متدهای کمکی پروژه شما)
        // فرض بر این است که متد ToLatinDate() خروجی DateTime برمی‌گرداند.
        DateTime targetDate = shamsiDate.ToLatinDate().Date;

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        // واکشی نوبت‌ها
        var appointmentsQuery = _db.Appointments
            .Include(a => a.Client)
            .Include(a => a.Lawyer).ThenInclude(l => l.User)
            .Where(a => a.AppointmentDate == targetDate);

        // واکشی جلسات دادگاه
        var sessionsQuery = _db.CourtSessions
            .Include(s => s.Case).ThenInclude(c => c.Client)
            .Include(s => s.Lawyer).ThenInclude(l => l.User)
            .Where(s => s.SessionDate.Date == targetDate);

        // اعمال فیلتر دسترسی بر اساس نقش
        if (role == "Lawyer")
        {
            appointmentsQuery = appointmentsQuery.Where(a => a.Lawyer.UserId == userId);
            sessionsQuery = sessionsQuery.Where(s => s.Lawyer.UserId == userId);
        }
        else if (role == "Client")
        {
            appointmentsQuery = appointmentsQuery.Where(a => a.Client.UserId == userId);
            sessionsQuery = sessionsQuery.Where(s => s.Case.Client.UserId == userId);
        }

        var dbAppointments = await appointmentsQuery.ToListAsync();
        var dbSessions = await sessionsQuery.ToListAsync();

        var scheduleList = new List<DailyScheduleItemViewModel>();

        // یکسان‌سازی نوبت‌ها
        scheduleList.AddRange(dbAppointments.Select(a => new DailyScheduleItemViewModel
        {
            Id = a.Id,
            Type = "Appointment",
            Time = a.StartTime.ToString(@"hh\:mm"),
            Title = "نوبت مشاوره/ملاقات",
            Description = a.Notes,
            ClientName = $"{a.Client.FirstName} {a.Client.LastName}",
            LawyerName = $"{a.Lawyer.User.FirstName} {a.Lawyer.User.LastName}",
            Status = (int)a.Status
        }));

        // یکسان‌سازی جلسات دادگاه
        scheduleList.AddRange(dbSessions.Select(s => new DailyScheduleItemViewModel
        {
            Id = s.Id,
            Type = "CourtSession",
            Time = s.SessionTime.ToString(@"hh\:mm"),
            Title = $"دادگاه: {s.CourtName} - شعبه {s.Branch}",
            Description = s.Notes,
            ClientName = $"{s.Case.Client.FirstName} {s.Case.Client.LastName}",
            LawyerName = $"{s.Lawyer.User.FirstName} {s.Lawyer.User.LastName}",
            Status = (int)s.Status
        }));

        // مرتب‌سازی بر اساس زمان و بازگشت به فرانت‌اند
        return Json(scheduleList.OrderBy(x => x.Time).ToList());
    }
}
