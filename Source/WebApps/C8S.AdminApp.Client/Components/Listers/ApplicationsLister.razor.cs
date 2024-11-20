using Blazr.RenderState;
using C8S.Domain.Enums;
using C8S.Domain.Models;
using C8S.Domain.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.Components.Listers;

public partial class ApplicationsLister : BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<ApplicationsLister> Logger { get; set; } = default!;

    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;
    #endregion

    #region Component Parameters
    [Parameter]
    public string? SortDescription { get; set; }

    [Parameter]
    public IList<ApplicationStatus>? Statuses { get; set; }
    #endregion

    #region Component Callbacks
    [Parameter]
    public EventCallback<int> TotalCountChanged { get; set; }
    #endregion

    #region Component References
    private Virtualize<ApplicationListDisplay>? _listerComponent;
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
    private async ValueTask<ItemsProviderResult<ApplicationListDisplay>>
        GetRows(ItemsProviderRequest request)
    {
        // shouldn't be called before prerender, but if it is...
        if (RenderStateService.IsPreRender) return default;

        try
        {
            var backendResponse = await Mediator.Send(new ListApplicationsQuery()
            {
                StartIndex = request.StartIndex,
                Count = request.Count,
                SortDescription = SortDescription,
                Statuses = Statuses
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            var results = backendResponse.Result!;
            await TotalCountChanged.InvokeAsync(results.Total);

            return new ItemsProviderResult<ApplicationListDisplay>(results.Items, results.Total);
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