using C8S.DrawDown.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SC.SendGrid.Abstractions.Models;
using SC.SendGrid.Services;

namespace C8S.Cleanup.Functions
{
    public class CleanC8SFunction(
        ILoggerFactory loggerFactory,
        DrawDownService drawDownService,
        EmailService emailService)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CleanC8SFunction>();

        [Function("CleanC8SFunction")]
        public async Task Run([TimerTrigger("0 30 5 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation("C8S Cleanup function started at: {time}", DateTime.UtcNow);

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
            var started = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var htmlMessage = await drawDownService.CleanC8S();
            var ended = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var fromEmail = new SimpleEmail("admin@bedtimemath.org", "Bedtime Math Admin");
            var toEmail = new SimpleEmail("dug@bedtimemath.org", "Dug Steen");
            var bodyHtml = "<html><body>" +
                           $"<div><b>Started</b>: {started:F}</div>" +
                           $"<div><b>Ended</b>: {ended:F}</div>" +
                           $"<div>{htmlMessage}</div>" +
                           "</body></html>";
            await emailService.SendEmail(fromEmail, toEmail, "C8S Draw Down Report", bodyHtml);

            _logger.LogInformation("C8S Cleanup function completed.");
        }
    }
}
