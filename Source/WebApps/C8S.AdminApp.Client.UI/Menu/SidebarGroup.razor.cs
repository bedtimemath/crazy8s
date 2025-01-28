using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Interfaces;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarGroup: BaseRazorComponent, IDisposable, ISetSelectable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarGroup> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public PageGroup Group { get; set; } = null!;
    
    [Parameter]
    public SidebarMenuCoordinator Coordinator { get; set; } = null!;

    [Parameter]
    public EventCallback<PageGroup> Clicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected = false;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Coordinator.RegisterComponent(Group.Url, this.SetSelected);
        base.OnInitialized();
    }
    public void Dispose()
    {
        Coordinator.UnregisterComponent(Group.Url);
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

}