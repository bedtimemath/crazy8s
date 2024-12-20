using C8S.AdminApp.Auth;
using Microsoft.AspNetCore.Mvc;
using C8S.Domain.Extensions;
using Serilog.Core;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[RequireAuthorizeKey]
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
            _logger.LogInformation("Ping controller called");

            var logLevel = levelSwitch.MinimumLevel;
            _logger.LogWarning("MinimumLevel:{LogLevel}", logLevel);
            _logger.LogAllLevels();

            return Ok(configuration.CreatePingOutput(
                [
                    new KeyValuePair<string, string>(
                        nameof(LoggingLevelSwitch.MinimumLevel),
                        levelSwitch.MinimumLevel.ToString())
                ]));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error while responding to ping");
            return Problem(exception.Message);
        }
    }
}