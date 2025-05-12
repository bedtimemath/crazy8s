using C8S.DrawDown.Services;
using C8S.DrawDownApp.Base;
using C8S.DrawDownApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC.SendGrid.Abstractions.Models;
using SC.SendGrid.Services;

namespace C8S.DrawDownApp.Tasks;

internal class RemoveC8SData(
    ILogger<RemoveC8SData> logger,
    RemoveC8SDataOptions options,
    EmailService emailService,
    DrawDownService drawDownService,
    IConfiguration configuration) : IActionLauncher
{
    public async Task<int> Launch()
    {
        // load the configurations that we need
        var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                          throw new Exception($"Missing configuration section: {Connections.SectionName}");

        logger.LogInformation("=== {Name} ===", nameof(RemoveC8SData));
        logger.LogInformation("Platform: {Platform}", options.Platform);
        logger.LogInformation("Database: {Database}", connections.Database);
        logger.LogInformation("OldSystem: {OldSystem}", connections.OldSystem);

        logger.LogInformation("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;
        Console.WriteLine();

        
        logger.LogInformation("C8S Cleanup function started at: {time}", DateTime.UtcNow);

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

        logger.LogInformation("{Name}: complete.", nameof(RemoveC8SData));
        return 0;
    }
}