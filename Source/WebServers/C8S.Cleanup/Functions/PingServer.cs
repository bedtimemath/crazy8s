using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using SC.SendGrid.Abstractions.Models;
using Serilog.Core;

namespace C8S.Functions.Functions;

public class PingServer(
    ILoggerFactory loggerFactory,
    IConfiguration configuration,
    LoggingLevelSwitch levelSwitch)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PingServer> _logger = loggerFactory.CreateLogger<PingServer>();
    #endregion

    [Function("PingServer")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        HttpResponseData response;
        try
        {
            _logger.LogInformation("PingServer triggered");
            response = req.CreateResponse(HttpStatusCode.OK);

            var sbOutput = new StringBuilder();

            // GENERAL
            sbOutput.Append("== General ==\r\n");
            sbOutput.AppendFormat("AppConfig: {0}\r\n", configuration.GetConnectionString("AppConfig")?.Obscure());
            sbOutput.AppendFormat("Environment: {0}\r\n", configuration["ENVIRONMENT"]);
            sbOutput.AppendFormat("LogLevel: {0}\r\n", levelSwitch.MinimumLevel);

            // EMAIL SETTINGS
            var emailSettings = new EmailSettings();
            configuration.GetSection(EmailSettings.SectionName).Bind(emailSettings);

            sbOutput.Append("== EmailSettings ==\r\n");
            sbOutput.AppendFormat("AdminEmails: {0}\r\n", emailSettings.AdminEmails);
            sbOutput.AppendFormat("UseProductionEmail: {0}\r\n", emailSettings.UseProductionEmail);
            sbOutput.Append("Lookup\r\n");
            foreach (var lookupKey in emailSettings.Lookup.Keys)
                sbOutput.AppendFormat("\t{0}: {1}\r\n", lookupKey, emailSettings.Lookup[lookupKey]?.Obscure());

            await response.WriteStringAsync(sbOutput.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            // can't use WriteAsJson, see: https://github.com/Azure/azure-functions-dotnet-worker/issues/344
            // await response.WriteAsJsonAsync(ex.ToSerializableException());
            await response.WriteStringAsync(JsonSerializer.Serialize(ex.ToSerializableException()));
        }
        return response;
    }
}