using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarSingleCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarSingleCoordinator> _logger = loggerFactory.CreateLogger<SidebarSingleCoordinator>();
    #endregion

    #region Public Properties
    public bool IsSelected { get; private set; }
    public MenuSingle Single { get; set; } = null!;
    #endregion

    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();

        PubSubService.Subscribe<NavigationChange>(HandleNavigationChange);
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
        var shouldBeSelected = Single.Url == navigationChange.PageUrl;
        if (shouldBeSelected == IsSelected) return;

        IsSelected = shouldBeSelected;
        if (ComponentRefresh != null)
            await ComponentRefresh.Invoke().ConfigureAwait(false);
    }
    #endregion

    #region Public Methods
    public async Task HandleClicked()
    {
        _logger.LogInformation("Single Clicked: {@Single}", Single);
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Open,
            PageUrl = Single.Url
        });
    }
    #endregion
}