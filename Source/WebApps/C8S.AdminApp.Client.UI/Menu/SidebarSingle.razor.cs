using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarSingle: BaseCoordinatedComponent<SidebarSingleCoordinator>, IDisposable
{
    #region Component Parameters
    [Parameter]
    public MenuSingle Single { get; set; } = null!;
    #endregion

    #region Private Variables
    //private ILogger<SidebarSingle> _logger = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //_logger = Service.LoggerFactory.CreateLogger<SidebarSingle>();
        Service.SetUp();
        Service.ComponentRefresh = async () => await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Service.TearDown();
        Service.ComponentRefresh = null;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Service.Single = Single;
    }
    #endregion
}