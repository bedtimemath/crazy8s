using C8S.AdminApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Audit.Abstractions.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using C8S.AdminApp.Common;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class DataChangesController(
    ILoggerFactory loggerFactory,
    IHubContext<CommunicationHub> hubContext) : ControllerBase
{
    private readonly ILogger<ApplicationsController> _logger = loggerFactory.CreateLogger<ApplicationsController>();

    [HttpPost]
    public async Task<IActionResult> PostDataChanges()
    {
        try
        {
            var bodyJson = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            var dataChange = JsonSerializer.Deserialize<DataChange>(bodyJson, options) ??
                             throw new Exception($"Could not deserialize data change: {bodyJson}");
            await hubContext.Clients.All.SendAsync(AdminAppConstants.Messages.DataChange, dataChange);

            return Accepted();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error while raising data change");
            return Problem(exception.Message);
        }
    }
}