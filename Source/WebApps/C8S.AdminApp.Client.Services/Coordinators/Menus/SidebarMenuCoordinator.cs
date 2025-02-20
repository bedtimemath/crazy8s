using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarMenuCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    //private readonly ILogger<SidebarMenuCoordinator> _logger = loggerFactory.CreateLogger<SidebarMenuCoordinator>();
    #endregion

    #region Public Methods
    public async Task<IEnumerable<MenuGroup>> GetMenuGroups()
    {
        var response = await GetQueryResults<MenuGroupsQuery, WrappedResponse<IEnumerable<MenuGroup>>>(
            new MenuGroupsQuery());
        return response.Success ? response.Result! : [];
    }
    public async Task<IEnumerable<MenuSingle>> GetMenuSingles(bool showBeforeOthers)
    {
        var response = await GetQueryResults<MenuSinglesQuery, WrappedResponse<IEnumerable<MenuSingle>>>(
            new MenuSinglesQuery() { ShowBeforeOthers = showBeforeOthers });
        return response.Success ? response.Result! : [];
    }
    #endregion
}