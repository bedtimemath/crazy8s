using C8S.AdminApp.Client.Services.Coordinators.Base;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Base;

public abstract class BaseRazorListContainer<TCoordinator, TListItem> : BaseRazorComponent
    where TCoordinator: BaseListCoordinator<TListItem> 
    where TListItem: class, new()

{
    #region Component Parameters
    public abstract TCoordinator Coordinator { get; set; }
    #endregion

    #region Component References
    protected Virtualize<TListItem> _listerComponent = null!;
    #endregion

    #region Private Variables
    protected bool _isBusy = true;
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