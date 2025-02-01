using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarItem: BaseCoordinatedComponent<SidebarItemCoordinator>
{
    #region Private Variables
    private ILogger<SidebarItem> _logger = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public MenuItem Item { get; set; } = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _logger = Service.LoggerFactory.CreateLogger<SidebarItem>();
        Service.PubSubService.Subscribe<NavigationChange>(Service.HandleNavigationChange);
        Service.ComponentRefresh = async () => await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Service.PubSubService.Unsubscribe<NavigationChange>(Service.HandleNavigationChange);
        Service.ComponentRefresh = null;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Service.Item = Item;
    }
    #endregion

    #region Event Handlers
    private void HandleCloseButtonClicked()
    {
        _logger.LogDebug("Close: {@Item}", Item);
    }
    #endregion
}