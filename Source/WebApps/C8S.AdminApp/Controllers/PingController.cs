using C8S.Domain.AppConfigs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SC.Common.Extensions;
using Serilog.Core;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController(
    ILoggerFactory loggerFactory,
    IConfiguration configuration,
    LoggingLevelSwitch levelSwitch) : ControllerBase
{
    private readonly ILogger<PingController> _logger = loggerFactory.CreateLogger<PingController>();

    [HttpPost]
    public IActionResult Index()
    {
        try
        {           
            var sbOutput = new StringBuilder();

            // GENERAL
            sbOutput.Append("== General ==\r\n");
            sbOutput.AppendFormat("Environment: {0}\r\n", configuration["ENVIRONMENT"]);
            sbOutput.AppendFormat("LogLevel: {0}\r\n", levelSwitch.MinimumLevel);

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

            // LOGGER TESTING
            _logger.LogWarning("MinimumLevel:{LogLevel}", levelSwitch.MinimumLevel);
            _logger.LogTrace("Trace");
            _logger.LogDebug("Debug");
            _logger.LogInformation("Information");
            _logger.LogWarning("Warning");
            _logger.LogError("Error");
            _logger.LogCritical("Critical");
            //throw new Exception("Something bad happened");

            return Ok(sbOutput.ToString());
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error while responding to ping");
            return Problem(exception.Message);
        }
    }
}