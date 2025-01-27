using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Services;
using SC.Common.PubSub;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseCoordinatedComponent<SidebarMenuCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenu> Logger { get; set; } = null!;

    [Inject]
    public PubSubService PubSubService { get; set; } = null!;

    [Inject]
    public PagesService PagesService { get; set; } = null!;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        PubSubService.Subscribe<PageChange>(HandlePageChangeNotification);

        base.OnInitialized();
    }

    public void Dispose()
    {
        PubSubService.Unsubscribe<PageChange>(HandlePageChangeNotification);
    }
    #endregion

    #region Event Handlers
    public async Task HandlePageChangeNotification(PageChange pageChange)
    {
        Logger.LogDebug("PageChange={@PageChange}", pageChange);
        await InvokeAsync(StateHasChanged);
    }
    #endregion
}