using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCQRSCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarItemCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemCoordinator>();
    #endregion
    
    #region Public Properties
    public bool IsSelected { get; private set; }
    public MenuItem Item { get; set; } = null!;
    #endregion
    
    #region Event Handlers
    public async Task HandleNavigationChange(NavigationChange navigationChange)
    {
        var shouldBeSelected = Item.Url == navigationChange.NewUrl;
        if (shouldBeSelected == IsSelected) return;

        IsSelected = shouldBeSelected;
        if (ComponentRefresh != null)
            await ComponentRefresh.Invoke().ConfigureAwait(false);
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
}