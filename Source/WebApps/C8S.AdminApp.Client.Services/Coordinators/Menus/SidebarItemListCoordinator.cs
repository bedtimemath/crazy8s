using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCQRSCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    //private readonly ILogger<SidebarItemListCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemListCoordinator>();
    #endregion
    
    #region Public Properties
    public MenuGroup Group { get; set; } = null!;
    #endregion

    #region Public Methods
    public async Task<IEnumerable<MenuItem>> GetMenuItems()
    {
        var response = await GetQueryResults<MenuItemsQuery, DomainResponse<IEnumerable<MenuItem>>>(
            new MenuItemsQuery() { Entity = Group.Entity });
        return response.Success ? response.Result! : [];
    }
    #endregion
}