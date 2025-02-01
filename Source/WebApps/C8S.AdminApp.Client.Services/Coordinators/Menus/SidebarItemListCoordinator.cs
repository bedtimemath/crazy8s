using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemListCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService) : BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarItemListCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemListCoordinator>();
    #endregion
}