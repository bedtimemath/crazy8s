using Blazr.RenderState;
using C8S.Domain.Enums;
using C8S.Domain.Models;
using C8S.Domain.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.Components.Listers;

public partial class RequestsLister : BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<RequestsLister> Logger { get; set; } = default!;

    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;
    #endregion

    #region Component Parameters
    [Parameter]
    public string? SortDescription { get; set; }

    [Parameter]
    public IList<RequestStatus>? Statuses { get; set; }
    #endregion

    #region Component Callbacks
    [Parameter]
    public EventCallback<int> TotalCountChanged { get; set; }
    #endregion

    #region Component References
    private Virtualize<RequestListDisplay>? _listerComponent;
    #endregion

    #region Public Methods
    public async Task Reload()
    {
        if (_listerComponent == null) return;

        await _listerComponent.RefreshDataAsync();
        await InvokeAsync(StateHasChanged);
    }
    #endregion

    #region Private Methods
    private async ValueTask<ItemsProviderResult<RequestListDisplay>>
        GetRows(ItemsProviderRequest request)
    {
        // shouldn't be called before prerender, but if it is...
        if (RenderStateService.IsPreRender) return default;

        try
        {
            var backendResponse = await Mediator.Send(new ListRequestsQuery()
            {
                StartIndex = request.StartIndex,
                Count = request.Count,
                SortDescription = SortDescription,
                Statuses = Statuses
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            var results = backendResponse.Result!;
            await TotalCountChanged.InvokeAsync(results.Total);

            return new ItemsProviderResult<RequestListDisplay>(results.Items, results.Total);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error Getting Rows");
            await RaiseExceptionAsync(ex).ConfigureAwait(false);
            return default;
        }
    }
    #endregion
}