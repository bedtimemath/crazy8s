using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace C8S.Cleanup.Functions;

public class SetLogLevel(
    ILoggerFactory loggerFactory,
    LoggingLevelSwitch levelSwitch)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SetLogLevel>();
    #endregion

    [Function("SetLogLevel")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Admin, "get")] HttpRequestData req)
    {
        HttpResponseData response;
        try
        {
            _logger.LogInformation("SetLogLevel triggered");

            LogEventLevel level;
            var levelString = req.Query["level"];
            if (String.IsNullOrEmpty(levelString))
                throw new ArgumentNullException(nameof(level));
            if (!Enum.TryParse(levelString, out level))
                throw new ArgumentException($"Unrecognized LogLevel: {levelString}", nameof(level));
            levelSwitch.MinimumLevel = level;

            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Minimum Level set to {levelSwitch.MinimumLevel}");
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