using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarItemList: BaseCoordinatedComponent<SidebarItemListCoordinator>
{
    #region Component Parameters
    [Parameter]
    public MenuGroup Group { get; set; } = null!;
    #endregion

    #region Private Variables
    //private ILogger<SidebarMenu> _logger = null!;
    private IEnumerable<MenuItem> _items = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //_logger = Service.LoggerFactory.CreateLogger<SidebarMenu>();
        Service.ComponentRefresh = async () => await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Service.ComponentRefresh = null;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        Service.Group = Group;
        _items = await Service.GetMenuItems();
    }
    #endregion
}