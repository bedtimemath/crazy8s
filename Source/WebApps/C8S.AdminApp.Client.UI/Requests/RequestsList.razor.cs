using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Requests.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsList : BaseClientComponent, IDisposable
{
    #region Injected Properties
    [Inject]
    public ILogger<RequestsList> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public RequestsListController Controller { get; set; } = null!;
    #endregion

    #region Component References
    private Virtualize<RequestListItem> _listerComponent = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Controller.FilterChanged += HandleFilterChanged;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Controller.FilterChanged -= HandleFilterChanged;
    }
    #endregion

    #region Event Handlers
    private void HandleFilterChanged(object? sender, EventArgs e) => 
        Task.Run(async () => await RefreshListerData());
    #endregion

    #region Private Methods
    private async Task RefreshListerData()
    {
        await _listerComponent.RefreshDataAsync();
        await InvokeAsync(StateHasChanged);
    }
    #endregion
}