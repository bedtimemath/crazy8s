using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Filters;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace C8S.AdminApp.Components.Listers;

public partial class CoachesLister : BaseRazorComponent
{
    [Inject]
    public ILogger<CoachesLister> Logger { get; set; } = default!;

    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    [Parameter]
    public EventCallback<int> TotalChanged { get; set; } = default!;

    private CoachFilter? _filter = null;

    private Virtualize<CoachDTO> _coachesList = default!;

    private bool _isLoading = false;

    public async Task SetFilter(CoachFilter filter)
    {
        _filter = filter;

        await _coachesList.RefreshDataAsync();
        StateHasChanged();
    }

    private async ValueTask<ItemsProviderResult<CoachDTO>> LoadCoaches(ItemsProviderRequest request)
    {
        _isLoading = true;

        try
        {
            var total = await Repository.GetCoachesCount(
                filter: _filter);
            var coaches = await Repository.GetCoaches(
                filter: _filter,
                startIndex: request.StartIndex, 
                takeCount: Math.Min(request.Count, total - request.StartIndex));

            await TotalChanged.InvokeAsync(total);

            return new ItemsProviderResult<CoachDTO>(coaches, total);
        }
        finally
        {
            _isLoading = false;
        }

    }
}