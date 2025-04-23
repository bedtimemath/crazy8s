using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.Base;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.PubSub.Services;
using SC.Common.Razor.Navigation.Commands;
using SC.Common.Razor.Navigation.Enums;
using SC.Common.Razor.Navigation.Models;
using SC.Common.Razor.Navigation.Queries;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarGroupCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarGroupCoordinator> _logger = loggerFactory.CreateLogger<SidebarGroupCoordinator>();
    #endregion
    
    #region Public Properties
    public bool IsSelected { get; private set; }
    public MenuGroup Group { get; set; } = null!;
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
        if (navigationChange.Action != NavigationAction.Open) return;
        await CheckSelfAgainstUrl(navigationChange.PageUrl);
    }

    #endregion
    
    #region Public Methods
    public async Task HandleClicked()
    {
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Open,
            PageUrl = Group.Url
        });
    }
    #endregion
    
    #region Private Methods
    private async Task CheckSelfAgainstUrl(string? url = null)
    {
        url ??= (await GetQueryResults<CurrentUrlQuery, WrappedResponse<string>>(new CurrentUrlQuery())).Result;

        var shouldBeSelected = Group.Url == url;
        if (shouldBeSelected == IsSelected) return;

        IsSelected = shouldBeSelected;
        await PerformComponentRefresh();
    }
    #endregion
}