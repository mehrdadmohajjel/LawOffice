using LawOffice.Data;
using Microsoft.EntityFrameworkCore;

namespace LawOffice.Services
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ReminderBackgroundService> _logger;

        public ReminderBackgroundService(IServiceProvider services,
            ILogger<ReminderBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessRemindersAsync();
                // هر ۶ ساعت یکبار چک کن
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }

        private async Task ProcessRemindersAsync()
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var sms = scope.ServiceProvider.GetRequiredService<ISmsService>();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var threeDays = today.AddDays(3);

            // یادآوری نوبت‌ها
            var appointments = await db.Appointments
                .Include(a => a.Client)
                .Include(a => a.Lawyer)
                .Where(a => a.Status == Data.Entities.AppointmentStatus.Reserved
                    && (a.AppointmentDate.Date == tomorrow || a.AppointmentDate.Date == threeDays))
                .ToListAsync();

            foreach (var apt in appointments)
            {
                var daysLeft = (apt.AppointmentDate.Date - today).Days;
                var msg = $"یادآوری: نوبت مشاوره شما در تاریخ " +
                          $"{apt.AppointmentDate:yyyy/MM/dd} ساعت {apt.StartTime} " +
                          $"({daysLeft} روز دیگر) برقرار است.";

                if (daysLeft == 1 && !apt.ReminderSent1Day)
                {
                    if (apt.Client?.PhoneNumber != null)
                        await sms.SendAsync(apt.Client.PhoneNumber, msg, "Appointment", apt.Id);
                    if (apt.Lawyer?.PhoneNumber != null)
                        await sms.SendAsync(apt.Lawyer.PhoneNumber, msg, "Appointment", apt.Id);
                    apt.ReminderSent1Day = true;
                }
                else if (daysLeft == 3 && !apt.ReminderSent3Days)
                {
                    if (apt.Client?.PhoneNumber != null)
                        await sms.SendAsync(apt.Client.PhoneNumber, msg, "Appointment", apt.Id);
                    if (apt.Lawyer?.PhoneNumber != null)
                        await sms.SendAsync(apt.Lawyer.PhoneNumber, msg, "Appointment", apt.Id);
                    apt.ReminderSent3Days = true;
                }
            }

            // یادآوری جلسات دادگاه
            var sessions = await db.CourtSessions
                .Include(s => s.Lawyer)
                .Include(s => s.Case).ThenInclude(c => c.Client)
                .Where(s => s.Status == Data.Entities.CourtSessionStatus.Scheduled
                    && (s.SessionDate.Date == tomorrow || s.SessionDate.Date == threeDays))
                .ToListAsync();

            foreach (var session in sessions)
            {
                var daysLeft = (session.SessionDate.Date - today).Days;
                var msg = $"یادآوری: جلسه دادگاه پرونده {session.Case.CaseCode} " +
                          $"در تاریخ {session.SessionDate:yyyy/MM/dd} ساعت {session.SessionTime} " +
                          $"در {session.CourtName} ({daysLeft} روز دیگر) برقرار است.";

                if (daysLeft == 1 && !session.ReminderSent1Day)
                {
                    if (session.Lawyer?.PhoneNumber != null)
                        await sms.SendAsync(session.Lawyer.PhoneNumber, msg, "CourtSession", session.Id);
                    if (session.Case?.Client?.PhoneNumber != null)
                        await sms.SendAsync(session.Case.Client.PhoneNumber, msg, "CourtSession", session.Id);
                    session.ReminderSent1Day = true;
                }
                else if (daysLeft == 3 && !session.ReminderSent3Days)
                {
                    if (session.Lawyer?.PhoneNumber != null)
                        await sms.SendAsync(session.Lawyer.PhoneNumber, msg, "CourtSession", session.Id);
                    if (session.Case?.Client?.PhoneNumber != null)
                        await sms.SendAsync(session.Case.Client.PhoneNumber, msg, "CourtSession", session.Id);
                    session.ReminderSent3Days = true;
                }
            }

            await db.SaveChangesAsync();
        }
    }  
}