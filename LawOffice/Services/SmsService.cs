using LawOffice.Data;
using LawOffice.Data.Entities;
using IPE.SmsIrClient; // اضافه شدن فضای نام sms.ir

namespace LawOffice.Services
{
    public interface ISmsService
    {
        Task SendAsync(string phone, string message, string refType, long refId);
    }

    public class SmsService : ISmsService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<SmsService> _logger;

        public SmsService(AppDbContext db, IConfiguration config, ILogger<SmsService> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string phone, string message, string refType, long refId)
        {
            var log = new SmsLog
            {
                PhoneNumber = phone,
                Message = message,
                ReferenceType = refType,
                ReferenceId = refId
            };

            try
            {
                // خواندن اطلاعات از appsettings.json
                string apiKey = _config["SmsIr:ApiKey"];
                long lineNumber = long.Parse(_config["SmsIr:LineNumber"]);

                SmsIr smsIr = new SmsIr(apiKey);

                // ارسال پیامک
                var bulkSendResult = await smsIr.BulkSendAsync(lineNumber, message, new string[] { phone });

                _logger.LogInformation("SMS sent to {Phone}. Result: {Result}", phone, bulkSendResult?.Data);
                log.IsSent = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS to {Phone}", phone);
                log.IsSent = false;
                log.ErrorMessage = ex.Message;
            }

            _db.SmsLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
