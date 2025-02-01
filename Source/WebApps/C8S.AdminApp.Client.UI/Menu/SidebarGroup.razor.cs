using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarGroup: BaseCoordinatedComponent<SidebarGroupCoordinator>, IDisposable
{
    #region Component Parameters
    [Parameter]
    public MenuGroup Group { get; set; } = null!;
    #endregion

    #region Private Variables
    //private ILogger<SidebarGroup> _logger = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //_logger = Service.LoggerFactory.CreateLogger<SidebarGroup>();
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

        Service.Group = Group;
    }
    #endregion
}