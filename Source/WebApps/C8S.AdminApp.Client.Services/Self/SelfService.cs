using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SC.Common;
using SC.Common.Models;

namespace C8S.AdminApp.Client.Services.Self;

public class SelfService(
    ILoggerFactory loggerFactory)
{
    private readonly ILogger<SelfService> _logger = loggerFactory.CreateLogger<SelfService>();
    private readonly Guid _uniqueId = Guid.NewGuid();

    public event EventHandler? SelfChanged = null;
    private void RaiseSelfChanged() => SelfChanged?.Invoke(this, EventArgs.Empty);

    public AppUser Self { get; set; } = new();

    public void SetIdentity(ClaimsPrincipal principal)
    {
        _logger.LogInformation("SetIdentity: {IsAuthenticated}", principal.Identity?.IsAuthenticated ?? false);

        if (principal.Identity is not { IsAuthenticated: true })
        { Self = new(); return; }

        Self.SessionId = Guid.NewGuid();
        Self.AuthIdentifier = _uniqueId.ToString("N"); //principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? SharedConstants.Display.NotSet;
        Self.EmailAddress = principal.FindFirst(ClaimTypes.Email)?.Value ?? SoftCrowConstants.Display.NotSet;
        Self.DisplayName = principal.FindFirst(ClaimTypes.Name)?.Value ??
                           principal.FindFirst("name")?.Value ??
                           SoftCrowConstants.Display.NotSet;

        _logger.LogInformation("Self: {@Self}", Self);

        RaiseSelfChanged();
    }
}
