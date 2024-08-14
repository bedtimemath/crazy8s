using C8S.AdminApp.Services;
using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace C8S.AdminApp.Components.Site;

public partial class SidebarSubmenuItem: BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarSubmenuItem> Logger { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public HistoryService HistoryService { get; set; } = default!;
    #endregion

    #region Component Parameters
    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public string? Page { get; set; }

    [Parameter]
    public string DisabledClass { get; set; } = "disabled";

    [Parameter]
    public string SelectedClass { get; set; } = "selected";

    [Parameter]
    public bool AutoLoadPage { get; set; } = true;
    #endregion

    #region Component Parameters (Callbacks)
    [Parameter] 
    public EventCallback MenuItemClicked { get; set; }

    [Parameter] 
    public EventCallback CloseButtonClicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected = false;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        NavigationManager.LocationChanged += HandleLocationChanged;
        HandleLocationChanged(null, new LocationChangedEventArgs(NavigationManager.Uri, false));
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
    #endregion

    #region Event Handlers

    private void HandleLocationChanged(object? _, LocationChangedEventArgs __)
    {
        var navUri = new Uri(NavigationManager.Uri);
        var pathOnly = navUri.GetLeftPart(UriPartial.Path).Substring(navUri.GetLeftPart(UriPartial.Authority).Length);
        _isSelected = String.Compare(pathOnly, Page, StringComparison.InvariantCultureIgnoreCase) == 0;
        
        StateHasChanged();
    }

    private async Task HandleComponentClicked()
    {
        try
        {
            if (IsDisabled) return;

            if (!AutoLoadPage)
                await MenuItemClicked.InvokeAsync().ConfigureAwait(false);
            else
            {
                if (String.IsNullOrEmpty(Page))
                    throw new Exception("When AutoLoadPage is true (default), Page must be set.");

                NavigationManager.NavigateTo(Page);
            }

        }
        catch (Exception ex)
        {
            await RaiseExceptionAsync(ex);
        }
    }
    #endregion
}