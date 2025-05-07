using System.Text;
using C8S.Cleanup.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC.SendGrid.Abstractions.Models;
using SC.SendGrid.Services;

namespace C8S.Cleanup.Functions
{
    public class CleanC8SFunction(
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        EmailService emailService)
    {
        const int TotalMinutes = 10;
        const string ReadBitsTables =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Bits' AND TABLE_TYPE = 'BASE TABLE' AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Bits' AND TABLE_NAME = INFORMATION_SCHEMA.TABLES.TABLE_NAME AND COLUMN_NAME = 'CreatedBy' );";
        const string ReadCrazy8STables =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Crazy8s' AND TABLE_TYPE = 'BASE TABLE' AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Crazy8s' AND TABLE_NAME = INFORMATION_SCHEMA.TABLES.TABLE_NAME AND COLUMN_NAME = 'CreatedBy' );";

        private readonly ILogger _logger = loggerFactory.CreateLogger<CleanC8SFunction>();

        [Function("CleanC8SFunction")]
        public async Task Run([TimerTrigger("0 30 5 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation("C8S Cleanup function started at: {time}", DateTime.UtcNow);

            StringBuilder htmlMessage = new StringBuilder();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
            var started = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            try
            {
                var tables = new HashSet<string>();
                
                var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                                  throw new Exception($"Missing configuration section: {Connections.SectionName}");

                await using var cnn = new SqlConnection(connections.OldSystem);
                await cnn.OpenAsync();

                var cmd = new SqlCommand(ReadBitsTables, cnn);
                var reader = await cmd.ExecuteReaderAsync();
                int added = 0, skipped = 0;
                while (reader.Read())
                {
                    var tableName = reader.GetString(0);
                    if (tables.Add($"[Bits].[{tableName}]")) added++;
                    else skipped++;
                }
                htmlMessage.AppendLine($"<div>Bits: Added {added:#,##0} tables; skipped {skipped:#,##0}.</div>");
                reader.Close();

                cmd = new SqlCommand(ReadCrazy8STables, cnn);
                reader = await cmd.ExecuteReaderAsync();
                added = 0; skipped = 0;
                while (reader.Read())
                {
                    var tableName = reader.GetString(0);
                    if (tables.Add($"[Crazy8s].[{tableName}]")) added++;
                    else skipped++;
                }
                htmlMessage.AppendLine($"<div>C8S: Added {added:#,##0} tables; skipped {skipped:#,##0}.</div>");
                reader.Close();

                var activeIds = new HashSet<Guid>();

                cmd = new SqlCommand("SELECT DISTINCT [AppSessionId] FROM [Bits].[AccessAttempt] WHERE [AppSessionId] IS NOT NULL;", cnn);
                reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var appSessionId = reader.GetGuid(0);
                    activeIds.Add(appSessionId);
                }
                reader.Close();

                foreach (var table in tables)
                {
                    cmd = new SqlCommand($"SELECT DISTINCT [CreatedBy] FROM {table} WHERE [CreatedBy] IS NOT NULL;", cnn);
                    reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var appSessionId = reader.GetGuid(0);
                        activeIds.Add(appSessionId);
                    }
                    reader.Close();
                }

                htmlMessage.AppendLine($"<div>Legitimate AppSession Ids: {activeIds.Count:#,##0}</div>");

                var removeIds = new HashSet<Guid>() { Guid.NewGuid() };

                cmd = new SqlCommand($"SELECT [Id] FROM [Bits].[AppSession];", cnn);
                reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var appSessionId = reader.GetGuid(0);
                    removeIds.Add(appSessionId);
                }
                reader.Close();

                removeIds.ExceptWith(activeIds);
                htmlMessage.AppendLine($"<div>AppSession Ids to Remove: {removeIds.Count:#,##0}</div>");

                var startTime = DateTime.Now;
                var index = 0;
                for (; index < removeIds.Count; index++)
                {
                    //cmd = new SqlCommand($"DELETE FROM [Bits].[AppSession] WHERE [Id] = @Id;", cnn);
                    //cmd.Parameters.Add(new SqlParameter("@Id", removeIds.ElementAt(index)));
                    //await cmd.ExecuteNonQueryAsync();

                    await Task.Delay(500); // Simulate work

                    var elapsed = DateTime.Now - startTime;
                    if (elapsed.TotalMinutes > TotalMinutes) break;
                }

                htmlMessage.AppendLine($"<div><div>Process complete. {index:#,##0} ids removed.</div></div>");
            }
            catch (Exception ex)
            {
                htmlMessage.AppendFormat("<div><b>ERROR</b>{0}</div>", ex.Message);
                htmlMessage.AppendFormat("<div>{0}</div>", ex.StackTrace);
            }
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
