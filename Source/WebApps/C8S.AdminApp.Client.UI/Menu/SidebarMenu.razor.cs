using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.Domain;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseCoordinatedComponent<SidebarMenuCoordinator>, IDisposable
{
    #region Private Variables
    //private ILogger<SidebarMenu> _logger = null!;
    private IEnumerable<MenuGroup> _groups = null!;
    private readonly MenuSingle _homeMenu = new()
    {
        Display = "Home",
        IconString = C8SConstants.Icons.Home,
        Url = "home"
    };
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //_logger = Service.LoggerFactory.CreateLogger<SidebarMenu>();
        Service.SetUp();
        Service.ComponentRefresh = async () => await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Service.TearDown();
        Service.ComponentRefresh = null;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _groups = await Service.GetMenuGroups();
    }
    #endregion
}