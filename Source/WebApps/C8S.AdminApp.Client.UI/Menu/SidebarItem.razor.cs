using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarItem: BaseCoordinatedComponent<SidebarItemCoordinator>
{
    #region Component Parameters
    [Parameter]
    public MenuItem Item { get; set; } = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Service.Item = Item;
    }
    #endregion
}