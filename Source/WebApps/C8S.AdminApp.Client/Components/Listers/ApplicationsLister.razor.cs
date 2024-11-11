using C8S.AdminApp.Client.Components.Base;
using C8S.Domain.Models;
using C8S.Domain.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace C8S.AdminApp.Client.Components.Listers;

public partial class ApplicationsLister : BaseRenderStateComponent
{
    [Inject]
    public ILogger<ApplicationsLister> Logger { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    private Virtualize<ApplicationListDisplay>? _listerComponent;

    public async Task Reload()
    {
        if (_listerComponent != null)
            await _listerComponent.RefreshDataAsync();
    }

    private async ValueTask<ItemsProviderResult<ApplicationListDisplay>>
        GetRows(ItemsProviderRequest request)
    {
        // shouldn't be called before prerender, but if it is...
        if (IsPreRender) return default;

        try
        {
            var backendResponse = await Mediator.Send(new ListApplicationsQuery()
            {
                StartIndex = request.StartIndex,
                Count = request.Count
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            var results = backendResponse.Result!;
            return new ItemsProviderResult<ApplicationListDisplay>(results.Items, results.Total);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error Getting Rows");
            await RaiseExceptionAsync(ex).ConfigureAwait(false);
            return default;
        }
    }
}