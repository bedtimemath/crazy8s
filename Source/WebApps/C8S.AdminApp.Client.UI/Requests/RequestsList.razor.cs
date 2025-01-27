using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.Domain.Features.Requests.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsList : BaseRazorComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<RequestsList> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public RequestsListCoordinator Coordinator { get; set; } = null!;
    #endregion

    #region Component References
    private Virtualize<RequestListItem> _listerComponent = null!;
    #endregion

    #region Private Variables
    private bool _isBusy = true;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Coordinator.FilterChanged += HandleFilterChanged;
        Coordinator.ListUpdated += HandleListUpdated;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Coordinator.FilterChanged -= HandleFilterChanged;
        Coordinator.ListUpdated -= HandleListUpdated;
    }
    #endregion

    #region Event Handlers
    private void HandleFilterChanged(object? sender, EventArgs e) => 
        Task.Run(RefreshListerData);

    private void HandleListUpdated(object? sender, EventArgs e) =>
        Task.Run(SetNotBusy);
    #endregion

    #region Private Methods
    private async Task RefreshListerData()
    {
        await SetBusy();
        await _listerComponent.RefreshDataAsync();
        await InvokeAsync(StateHasChanged);
        await Coordinator.ScrollListToTop();
    }

    private async Task SetBusy() => await SetBusy(true);
    private async Task SetNotBusy() => await SetBusy(false);
    private async Task SetBusy(bool isBusy)
    {
        await InvokeAsync(() =>
        {
            _isBusy = isBusy;
            StateHasChanged();
        });
    }
    #endregion
}