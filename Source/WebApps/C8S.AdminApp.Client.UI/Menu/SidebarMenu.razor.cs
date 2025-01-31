using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseCoordinatedComponent<SidebarMenuCoordinator>, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILoggerFactory LoggerFactory { get; set; } = null!;

    [Inject]
    public IPubSubService PubSubService { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    #endregion
    
    #region Component References
    private SidebarGroup _requestsGroup = null!;
    private SidebarGroup _contactsGroup = null!;
    private SidebarGroup _organizationsGroup = null!;
    private SidebarGroup _sitesGroup = null!;
    private SidebarGroup _ordersGroup = null!;
    private SidebarGroup _skusGroup = null!;
    #endregion
    
    #region Private Variables
    private ILogger<SidebarMenu> _logger = null!;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _logger = LoggerFactory.CreateLogger<SidebarMenu>();

        PubSubService.Subscribe<NavigationChange>(HandlePageChangeNotification);

        _ = UpdateSelecteds();
    }

    public void Dispose()
    {
        PubSubService.Unsubscribe<NavigationChange>(HandlePageChangeNotification);
    }
    #endregion
    
    #region Event Handlers
    private async Task HandlePageChangeNotification(NavigationChange navigationChange)
    {
        _logger.LogDebug("PageChange={@PageChange}", navigationChange);
        await UpdateSelecteds();
    }
    private async Task HandleSidebarGroupClicked(NavigationGroup group)
    {
        var pageUrl = group switch
        {
            NavigationGroup.Requests => "/requests",
            NavigationGroup.Contacts => "/contacts",
            NavigationGroup.Sites => "/sites",
            NavigationGroup.Organizations => "/organizations",
            NavigationGroup.Orders => "/orders",
            NavigationGroup.Skus => "/skus",
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
        };
        await Service.HandleSidebarGroupClicked(group, pageUrl);
    }
    #endregion
    
    #region Private Methods
    private async Task UpdateSelecteds()
    {
        var group = NavigationManager.GetGroup();

        _logger.LogDebug("UpdateSelected: {Group}", group);

        await _requestsGroup.SetSelected(group == NavigationGroup.Requests);
        await _contactsGroup.SetSelected(group == NavigationGroup.Contacts);
        await _sitesGroup.SetSelected(group == NavigationGroup.Sites);
        await _organizationsGroup.SetSelected(group == NavigationGroup.Organizations);
        await _ordersGroup.SetSelected(group == NavigationGroup.Orders);
        await _skusGroup.SetSelected(group == NavigationGroup.Skus);
    }
    #endregion
}