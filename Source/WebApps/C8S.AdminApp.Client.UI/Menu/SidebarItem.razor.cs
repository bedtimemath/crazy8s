using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarItem: BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarItem> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public PageItem Item { get; set; } = null!;
    
    [Parameter]
    public SidebarMenuCoordinator Coordinator { get; set; } = null!;

    [Parameter]
    public EventCallback<PageItem> Clicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Coordinator.RegisterComponent(Item.Url, this.SetSelected);
        base.OnInitialized();
    }
    public void Dispose()
    {
        Coordinator.UnregisterComponent(Item.Url);
    }
    #endregion

    #region Public Methods
    public void SetSelected(bool isSelected)
    {
        if (_isSelected == isSelected) return;

        _isSelected = isSelected;
        StateHasChanged();
    }
    #endregion

    #region Event Handlers
    private void HandleCloseButtonClicked()
    {
        Logger.LogDebug("Close: {@Item}", Item);
    }
    #endregion
}