using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenuItem: BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenuItem> Logger { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;
    #endregion

    #region Component Parameters
    [Parameter]
    public string? Display { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public string? PageRoute { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected = false;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        NavigationManager.LocationChanged += HandleLocationChanged;
        UpdateClassByLocation();
    }
    
    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
    #endregion

    #region Event Handlers
    private void HandleMenuItemClicked() =>
        NavigationManager.NavigateTo(PageRoute ?? "/");

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e) =>
        UpdateClassByLocation();
    #endregion

    #region Private Methods
    private void UpdateClassByLocation()
    {
        _isSelected = String.IsNullOrEmpty(PageRoute) ? NavigationManager.Uri == NavigationManager.BaseUri :
            (new Uri(NavigationManager.Uri)).PathAndQuery.StartsWith(PageRoute);
        StateHasChanged();
    }
    #endregion
}