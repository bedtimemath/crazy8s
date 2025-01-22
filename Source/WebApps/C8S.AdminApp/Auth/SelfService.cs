using SC.Common;

namespace C8S.AdminApp.Auth;

public class SelfService(
    ILoggerFactory loggerFactory,
    IHttpContextAccessor contextAccessor) : ISelfService
{
    //private readonly ILogger<SelfService> _logger = loggerFactory.CreateLogger<SelfService>();

    public string DisplayName
    {
        get
        {
            var user = contextAccessor.HttpContext?.User;
            return (!String.IsNullOrEmpty(user?.Identity?.Name)) ?
                user.Identity.Name : SoftCrowConstants.Display.None;
        }
    }
}