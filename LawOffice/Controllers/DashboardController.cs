using LawOffice.Data;
using LawOffice.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
// Using های مربوط به پروژه‌ی خودتان را اینجا قرار دهید (مثل AppDbContext و Model ها)

public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db) => _db = db;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalClients = await _db.Clients.CountAsync();
        ViewBag.ActiveCases = await _db.Cases.CountAsync(c => c.CaseStatusId == 1);
        ViewBag.TodayAppointments = await _db.Appointments.CountAsync(a => a.AppointmentDate == DateTime.Today);
        ViewBag.UpcomingSessions = await _db.CourtSessions.CountAsync(s => s.SessionDate >= DateTime.Today && s.Status == CourtSessionStatus.Scheduled);

        return View();
    }

    [HttpGet]
    [Authorize] // <--- اضافه شد
    public async Task<IActionResult> GetStats()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleStr = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(roleStr))
            return Unauthorized(new { success = false, message = "کاربر نامعتبر است" });

        var userId = long.Parse(userIdStr);
        var role = Enum.Parse<UserRole>(roleStr);

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        object data;

        if (role == UserRole.Staff)
        {
            data = new
            {
                totalClients = await _db.Clients.CountAsync(),
                activeCases = await _db.Cases.CountAsync(c => c.CaseStatusId == 1),
                todayAppointments = await _db.Appointments.CountAsync(a =>
                    a.AppointmentDate >= today && a.AppointmentDate < tomorrow &&
                    a.Status == AppointmentStatus.Reserved),
                upcomingSessions = await _db.CourtSessions.CountAsync(s =>
                    s.SessionDate >= today &&
                    s.Status == CourtSessionStatus.Scheduled)
            };
        }
        else if (role == UserRole.Lawyer)
        {
            var lawyer = await _db.Lawyers.FirstOrDefaultAsync(l => l.UserId == userId);
            if (lawyer == null)
                return Json(new { success = false, message = "وکیل یافت نشد" });

            data = new
            {
                totalClients = 0,
                activeCases = await _db.Cases.CountAsync(c =>
                    c.LawyerId == lawyer.Id && c.CaseStatusId == 1),
                todayAppointments = await _db.Appointments.CountAsync(a =>
                    a.LawyerId == lawyer.Id &&
                    a.AppointmentDate >= today && a.AppointmentDate < tomorrow &&
                    a.Status == AppointmentStatus.Reserved),
                upcomingSessions = await _db.CourtSessions.CountAsync(s =>
                    s.LawyerId == lawyer.Id &&
                    s.SessionDate >= today &&
                    s.Status == CourtSessionStatus.Scheduled)
            };
        }
        else // Client
        {
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
                return Json(new { success = false, message = "موکل یافت نشد" });

            data = new
            {
                totalClients = 0,
                activeCases = await _db.Cases.CountAsync(c => c.ClientId == client.Id),
                todayAppointments = await _db.Appointments.CountAsync(a =>
                    a.ClientId == client.Id &&
                    a.AppointmentDate >= today && a.AppointmentDate < tomorrow &&
                    a.Status == AppointmentStatus.Reserved),
                upcomingSessions = 0
            };
        }

        return Json(new { success = true, data });
    }

    [HttpGet]
    [Authorize] // <--- اضافه شد
    public async Task<IActionResult> GetRecentActivities() 
    {
        // فعلاً یک لیست خالی برمی‌گردونیم
        var items = new List<object>();

        return Json(new { success = true, items });
    }

    [HttpGet]
    [Authorize] // <--- اضافه شد
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleStr = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(roleStr))
            return Unauthorized();

        var userId = long.Parse(userIdStr);
        var role = Enum.Parse<UserRole>(roleStr);

        var today = DateTime.Today;
        var items = new List<object>();

        if (role == UserRole.Lawyer)
        {
            var lawyer = await _db.Lawyers.FirstOrDefaultAsync(l => l.UserId == userId);
            if (lawyer != null)
            {
                var appointments = await _db.Appointments
                    .Where(a => a.LawyerId == lawyer.Id && a.AppointmentDate >= today)
                    .OrderBy(a => a.AppointmentDate)
                    .Take(5)
                    .Select(a => new
                    {
                        title = $"قرار ملاقات با {a.Client.FirstName} {a.Client.LastName}",
                        date = a.AppointmentDate.ToString("yyyy/MM/dd HH:mm"),
                        startTime = a.StartTime
                    })
                    .ToListAsync();

                var sessions = await _db.CourtSessions
                    .Where(s => s.LawyerId == lawyer.Id && s.SessionDate >= today)
                    .OrderBy(s => s.SessionDate)
                    .Take(5)
                    .Select(s => new
                    {
                        title = $"جلسه دادگاه - {s.CourtName}",
                        date = s.SessionDate.ToString("yyyy/MM/dd"),
                        startTime =s.SessionTime
                    })
                    .ToListAsync();

                items.AddRange(appointments);
                items.AddRange(sessions);
            }
        }
        else if (role == UserRole.Client)
        {
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client != null)
            {
                var appointments = await _db.Appointments
                    .Where(a => a.ClientId == client.Id && a.AppointmentDate >= today)
                    .OrderBy(a => a.AppointmentDate)
                    .Take(5)
                    .Select(a => new
                    {
                        title = $"قرار ملاقات با {a.Lawyer.FirstName} {a.Lawyer.LastName}",
                        date = a.AppointmentDate.ToString("yyyy/MM/dd HH:mm"),
                        startTime = a.StartTime
                    })
                    .ToListAsync();

                items.AddRange(appointments);
            }
        }
        else // Staff
        {
            var appointments = await _db.Appointments
                .Where(a => a.AppointmentDate >= today)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new
                {
                    title = $"قرار ملاقات: {a.Client.FirstName} {a.Client.LastName} - {a.Lawyer.FirstName} {a.Lawyer.LastName}",
                    date = a.AppointmentDate.ToString("yyyy/MM/dd HH:mm"),
                    startTime =a.StartTime
                })
                .ToListAsync();

            items.AddRange(appointments);
        }

        // مرتب‌سازی نهایی همه آیتم‌ها بر اساس تاریخ
        items = items.OrderBy(x => ((dynamic)x).date).ToList();

        return Json(new { success = true, items });
    }
}
