using System.Net;
using System.Text;
using System.Text.Json;
using C8S.Domain.AppConfigs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.Functions.Functions;

public class PingServer(
    ILoggerFactory loggerFactory,
    IConfiguration configuration)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<PingServer>();
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
            sbOutput.AppendFormat("Environment: {0}\r\n", configuration["ENVIRONMENT"]);

            // CONNECTIONS
            var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                              throw new Exception($"Missing configuration section: {Connections.SectionName}");
            sbOutput.Append("== Connections ==\r\n");
            sbOutput.AppendFormat("Audit: {0}\r\n", connections.Audit?.Obscure());
            sbOutput.AppendFormat("ApplicationInsights: {0}\r\n", connections.ApplicationInsights?.Obscure());
            sbOutput.AppendFormat("AzureStorage: {0}\r\n", connections.AzureStorage?.Obscure());
            sbOutput.AppendFormat("Database: {0}\r\n", connections.Database?.Obscure());
            sbOutput.AppendFormat("OldSystem: {0}\r\n", connections.OldSystem?.Obscure());

            // API KEYS
            var apiKeys = new ApiKeys();
            configuration.GetSection(ApiKeys.SectionName).Bind(apiKeys);

            sbOutput.Append("== ApiKeys ==\r\n");
            sbOutput.AppendFormat("FullSlate: {0}\r\n", apiKeys.FullSlate?.Obscure());

            // EMAIL SETTINGS
            //var emailSettings = new EmailSettings();
            //configuration.GetSection(EmailSettings.SectionName).Bind(emailSettings);

            //sbOutput.Append("== EmailSettings ==\r\n");
            //sbOutput.AppendFormat("AdminEmails: {0}\r\n", emailSettings.AdminEmails);
            //sbOutput.AppendFormat("UseProductionEmail: {0}\r\n", emailSettings.UseProductionEmail);
            //sbOutput.AppendFormat("AdminUrl: {0}\r\n", emailSettings.AdminUrl?.Obscure());
            //sbOutput.AppendFormat("FunctionsUrl: {0}\r\n", emailSettings.FunctionsUrl?.Obscure());
            //sbOutput.AppendFormat("PublicUrl: {0}\r\n", emailSettings.PublicUrl?.Obscure());
            //sbOutput.AppendFormat("VolunteerUrl: {0}\r\n", emailSettings.VolunteerUrl?.Obscure());

            // EMAIL TEMPLATES
            //var emailTemplates = new EmailTemplates();
            //configuration.GetSection(EmailTemplates.SectionName).Bind(emailTemplates);

            //sbOutput.Append("== EmailTemplates ==\r\n");
            //sbOutput.AppendFormat("CheckOut: {0}\r\n", emailTemplates.CheckOut?.Obscure());
            //sbOutput.AppendFormat("ExtensionDayBefore: {0}\r\n", emailTemplates.ExtensionDayBefore?.Obscure());
            //sbOutput.AppendFormat("ExtensionGranted: {0}\r\n", emailTemplates.ExtensionGranted?.Obscure());
            //sbOutput.AppendFormat("LoanOverdue: {0}\r\n", emailTemplates.LoanOverdue?.Obscure());
            //sbOutput.AppendFormat("ManualHandOff: {0}\r\n", emailTemplates.ManualHandOff?.Obscure());
            //sbOutput.AppendFormat("ReminderDayBefore: {0}\r\n", emailTemplates.ReminderDayBefore?.Obscure());
            //sbOutput.AppendFormat("ReminderWeekBefore: {0}\r\n", emailTemplates.ReminderWeekBefore?.Obscure());
            //sbOutput.AppendFormat("MemberSurveyRequest: {0}\r\n", emailTemplates.MemberSurveyRequest?.Obscure());

            // ENDPOINTS
            var endpoints = new Endpoints();
            configuration.GetSection(Endpoints.SectionName).Bind(endpoints);

            sbOutput.Append("== Endpoints ==\r\n");
            sbOutput.AppendFormat("AzureStorage: {0}\r\n", endpoints.AzureStorage?.Obscure());
            sbOutput.AppendFormat("FullSlateApi: {0}\r\n", endpoints.FullSlateApi?.Obscure());

            // FUNCTION KEYS
            //var functionKeys = new FunctionKeys();
            //configuration.GetSection(FunctionKeys.SectionName).Bind(functionKeys);

            //sbOutput.Append("== FunctionKeys ==\r\n");
            //sbOutput.AppendFormat("CreateLabels: {0}\r\n", functionKeys.CreateLabels?.Obscure());
            //sbOutput.AppendFormat("ImageUpload: {0}\r\n", functionKeys.ImageUpload?.Obscure());
            //sbOutput.AppendFormat("QRCoder: {0}\r\n", functionKeys.QRCoder?.Obscure());

            // FUNCTION SETTINGS
            //var functionSettings = new FunctionSettings();
            //configuration.GetSection(FunctionSettings.SectionName).Bind(functionSettings);

            //sbOutput.Append("== FunctionSettings ==\r\n");
            //sbOutput.AppendFormat("RemoteImagePrefix: {0}\r\n", functionSettings.RemoteImagePrefix);
            //sbOutput.AppendFormat("RemoteImageBatchSize: {0}\r\n", functionSettings.RemoteImageBatchSize);

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