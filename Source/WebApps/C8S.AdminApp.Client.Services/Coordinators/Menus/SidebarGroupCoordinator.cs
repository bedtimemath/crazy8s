using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarGroupCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService) : BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarGroupCoordinator> _logger = loggerFactory.CreateLogger<SidebarGroupCoordinator>();
    #endregion
}