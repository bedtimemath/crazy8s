using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarMenuCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService): BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuCoordinator> _logger = loggerFactory.CreateLogger<SidebarMenuCoordinator>();
    #endregion

    #region Public Methods
    public async Task<IEnumerable<NavigationGroup>> GetNavigationGroups()
    {
        var response = await GetQueryResults<NavigationGroupsQuery, DomainResponse<IEnumerable<NavigationGroup>>>(
            new NavigationGroupsQuery());
        return response.Success ? response.Result! : [];
    }

    #endregion

    #region Event Handlers
    public Task HandlePageChangeNotification(NavigationChange navigationChange)
    {
        _logger.LogDebug("PageChange={@PageChange}", navigationChange);
        return Task.CompletedTask;
    }
    public async Task HandleSidebarGroupClicked(NavigationEntity entity, string pageUrl) =>
        await ExecuteCommand(new NavigationCommand() { Action = NavigationAction.Open, Entity = entity, PageUrl = pageUrl });

    public async Task HandleSidebarItemClicked(PageItem pageItem)
    {
        //await ExecuteCommand(new OpenNavigationCommand() { Group = NavigationGroup.Requests });
    }
    #endregion
}