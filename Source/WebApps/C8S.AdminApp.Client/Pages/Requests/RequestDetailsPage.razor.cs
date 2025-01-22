using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public sealed partial class RequestDetailsPage :
    BaseOwningRazorPage<RequestDetailsCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<RequestDetailsPage> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public int RequestId { get; set; }
    #endregion

    #region Private Variables
    private bool _isBusy = false;
    #endregion

    #region Component LifeCycle
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (!RendererInfo.IsInteractive) return;

        _isBusy = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Service.SetIdAsync(RequestId);
        }
        finally
        {
            _isBusy = false;
            await InvokeAsync(StateHasChanged);
        }
    } 
    #endregion
}