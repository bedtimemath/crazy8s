using System.Diagnostics.Metrics;
using C8S.AdminApp.Services;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Site;

public partial class SidebarMenu: BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public HistoryService HistoryService { get; set; } = default!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();
        HistoryService.Changed += HandleHistoryChanged;
    }

    public void Dispose()
    {
        HistoryService.Changed -= HandleHistoryChanged;
    }
    #endregion

    #region Event Handlers
    public void HandleHistoryChanged(object? sender, HistoryEventArgs eventArgs) =>
        InvokeAsync(StateHasChanged);

    private void HandleApplicationClosed(ApplicationDTO application) =>
        HistoryService.Remove(application);

    #endregion
}