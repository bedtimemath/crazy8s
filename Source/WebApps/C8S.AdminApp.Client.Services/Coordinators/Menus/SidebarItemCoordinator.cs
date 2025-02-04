using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarItemCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemCoordinator>();
    #endregion
    
    #region Public Properties
    public bool IsSelected { get; private set; }
    public MenuItem Item { get; set; } = null!;
    #endregion
    
    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();

        PubSubService.Subscribe<NavigationChange>(HandleNavigationChange);
        Task.Run(async () => await CheckSelfAgainstUrl());
    }

    public override void TearDown()
    {
        base.TearDown();

        PubSubService.Unsubscribe<NavigationChange>(HandleNavigationChange);
    }
    #endregion

    #region Event Handlers
    public async Task HandleNavigationChange(NavigationChange navigationChange)
    {
        _logger.LogDebug("Coordinator for {@Item}: {@Change}", Item, navigationChange);
        await CheckSelfAgainstUrl(navigationChange.PageUrl);
    }
    #endregion
    
    #region Public Methods
    public async Task HandleClicked()
    {
        _logger.LogInformation("Item Clicked: {@Item}", Item);
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Open,
            Entity = Item.Entity,
            PageUrl = Item.Url
        });
    }
    #endregion
    
    #region Private Methods
    private async Task CheckSelfAgainstUrl(string? url = null)
    {
        url ??= (await GetQueryResults<CurrentUrlQuery, DomainResponse<string>>(new CurrentUrlQuery())).Result;

        var shouldBeSelected = Item.Url == url;
        if (shouldBeSelected == IsSelected) return;

        IsSelected = shouldBeSelected;
        if (ComponentRefresh != null)
            await ComponentRefresh.Invoke().ConfigureAwait(false);
    }
    #endregion
}