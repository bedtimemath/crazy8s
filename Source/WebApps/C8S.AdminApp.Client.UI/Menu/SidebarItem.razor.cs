using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarItem: BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarItem> Logger { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public PageGroup Group { get; set; } = null!;

    [Parameter]
    public EventCallback<PageGroup> Clicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += HandleLocationChanged;
        CheckIsSelected();

        base.OnInitialized();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
    #endregion

    #region Event Handlers
    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e) => CheckIsSelected();
    #endregion

    #region Private Methods

    private void CheckIsSelected()
    {
        var currentUrl = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var shouldBeSelected = Group.Url == currentUrl;

        if (_isSelected == shouldBeSelected) return;

        _isSelected = shouldBeSelected;
        StateHasChanged();
    }
    #endregion
}