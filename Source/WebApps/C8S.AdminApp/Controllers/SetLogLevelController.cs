using C8S.AdminApp.Auth;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Serilog.Events;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[RequireAuthorizeKey]
public class SetLogLevelController(
    ILoggerFactory loggerFactory,
    LoggingLevelSwitch levelSwitch) : ControllerBase
{
    private readonly ILogger<SetLogLevelController> _logger = loggerFactory.CreateLogger<SetLogLevelController>();

    [HttpPost]
    public IActionResult Index(
        [FromQuery(Name = "level")] string? levelString)
    {
        try
        {           
            LogEventLevel level;
            if (String.IsNullOrEmpty(levelString))
                throw new ArgumentNullException(nameof(level));
            if (!Enum.TryParse(levelString, out level))
                throw new ArgumentException($"Unrecognized LogLevel: {levelString}", nameof(level));
            levelSwitch.MinimumLevel = level;

            return Ok($"Minimum Level set to {levelSwitch.MinimumLevel}");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error while responding to SetLogLevel");
            return Problem(exception.Message);
        }
    }
}