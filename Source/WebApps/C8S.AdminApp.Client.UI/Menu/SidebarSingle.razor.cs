using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarSingle: BaseCoordinatedComponent<SidebarSingleCoordinator>
{
    #region Component Parameters
    [Parameter]
    public MenuSingle Single { get; set; } = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Service.Single = Single;
    }
    #endregion
}