using System.Net;
using System.Text.Json;
using C8S.Domain.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using Serilog.Core;

namespace C8S.Functions.Functions;

public class PingServer(
    ILoggerFactory loggerFactory,
    IConfiguration configuration,
    LoggingLevelSwitch levelSwitch)
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

            _logger.LogWarning("MinimumLevel:{LogLevel}", levelSwitch.MinimumLevel);
            _logger.LogAllLevels();

            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(configuration.CreatePingOutput(
                [
                    new KeyValuePair<string, string>(
                        nameof(LoggingLevelSwitch.MinimumLevel),
                        levelSwitch.MinimumLevel.ToString())
                ] ));
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