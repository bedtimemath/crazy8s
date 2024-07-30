using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Filters;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace C8S.AdminApp.Components.Listers;

public partial class ApplicationsLister : BaseRazorComponent
{
    [Inject]
    public ILogger<ApplicationsLister> Logger { get; set; } = default!;

    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    private ApplicationFilter? _filter = null;

    private Virtualize<ApplicationDTO> _applicationsList = default!;

    private bool _isLoading = false;

    public async Task SetFilter(ApplicationFilter filter)
    {
        _filter = filter;

        await _applicationsList.RefreshDataAsync();
        StateHasChanged();
    }

    private async ValueTask<ItemsProviderResult<ApplicationDTO>> LoadApplications(ItemsProviderRequest request)
    {
        _isLoading = true;

        try
        {
            var total = await Repository.GetApplicationsCount();
            var toTake = Math.Min(request.Count, total - request.StartIndex);
            var applications = await Repository.GetApplications(
                filter: _filter,
                startIndex: request.StartIndex, takeCount: toTake);

            return new ItemsProviderResult<ApplicationDTO>(applications, total);
        }
        finally
        {
            _isLoading = false;
        }

    }
}