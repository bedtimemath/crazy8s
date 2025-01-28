using C8S.AdminApp.Client.Services.Coordinators.Menus;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseCoordinatedComponent<SidebarMenuCoordinator>, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenu> Logger { get; set; } = null!;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Service.SetRefreshMenu(Refresh);
        base.OnInitialized();
    }

    public void Dispose()
    {
        Service.ClearRefreshMenu();
    }
    #endregion
    
    #region Private Methods
    private async Task Refresh() => await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    #endregion
}