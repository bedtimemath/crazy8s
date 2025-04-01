using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.Extensions.Logging;
using SC.Common;
using SC.Common.Client.Navigation.Commands;
using SC.Common.Client.Navigation.Enums;
using SC.Common.Client.Navigation.Models;
using SC.Common.Client.Navigation.Queries;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    //private readonly ILogger<SidebarItemCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemCoordinator>();
    #endregion
    
    #region Public Properties
    public bool IsSelected { get; private set; }
    public MenuItem Item { get; set; } = null!;

    public string? Display { get; private set; }
    #endregion
    
    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();

        PubSubService.Subscribe<NavigationChange>(HandleNavigationChange);
        Task.Run(async () => await GetItemDisplay().ConfigureAwait(false));
        Task.Run(async () => await CheckSelfAgainstUrl().ConfigureAwait(false));
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
    public async Task HandleClicked() =>
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Open,
            PageUrl = DomainUrlEx.CreateUrlFromEntityIdValue(Item) 
        });
    public async Task HandleCloseClicked() =>
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Close,
            PageUrl = DomainUrlEx.CreateUrlFromEntityIdValue(Item) 
        });
    #endregion
    
    #region Private Methods

    private async Task GetItemDisplay()
    {
        throw new NotImplementedException();
#if false
        var response = await GetQueryResults<RequestTitleQuery, WrappedResponse<string?>>(
            new RequestTitleQuery() { RequestId = Item.IdValue });
        Display = response.Result ?? SoftCrowConstants.Display.NotSet;
        await PerformComponentRefresh();
#endif
    }
    private async Task CheckSelfAgainstUrl(string? url = null)
    {
        url ??= (await GetQueryResults<CurrentUrlQuery, WrappedResponse<string>>(new CurrentUrlQuery())).Result;

        var shouldBeSelected = DomainUrlEx.CreateUrlFromEntityIdValue(Item) == url;
        if (shouldBeSelected == IsSelected) return;

        IsSelected = shouldBeSelected;
        await PerformComponentRefresh();
    }
    #endregion
}