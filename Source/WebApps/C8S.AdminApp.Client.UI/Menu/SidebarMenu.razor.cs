using C8S.AdminApp.Client.Services.Coordinators.Menus;
using Microsoft.AspNetCore.Components;
using SC.Common.PubSub;
using SC.Common.Razor.Base;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseCoordinatedComponent<SidebarMenuCoordinator>, IDisposable
{
    #region Injected Properties
    [Inject]
    public IPubSubService PubSubService { get; set; } = null!;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        PubSubService.Subscribe<PageChange>(Service.HandlePageChangeNotification);
        Service.SetRefreshMenu(Refresh);
        base.OnInitialized();
    }

    public void Dispose()
    {
        PubSubService.Unsubscribe<PageChange>(Service.HandlePageChangeNotification);
        Service.ClearRefreshMenu();
    }
    #endregion
    
    #region Private Methods
    private async Task Refresh() => await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    #endregion
}