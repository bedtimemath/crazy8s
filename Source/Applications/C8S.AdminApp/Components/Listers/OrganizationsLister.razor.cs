using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Filters;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace C8S.AdminApp.Components.Listers;

public partial class OrganizationsLister : BaseRazorComponent
{
    [Inject]
    public ILogger<OrganizationsLister> Logger { get; set; } = default!;

    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    [Parameter]
    public EventCallback<int> TotalChanged { get; set; } = default!;

    private OrganizationFilter? _filter = null;

    private Virtualize<OrganizationDTO> _organizationsList = default!;

    private bool _isLoading = false;

    public async Task SetFilter(OrganizationFilter filter)
    {
        _filter = filter;

        await _organizationsList.RefreshDataAsync();
        StateHasChanged();
    }

    private async ValueTask<ItemsProviderResult<OrganizationDTO>> LoadOrganizations(ItemsProviderRequest request)
    {
        _isLoading = true;

        try
        {
            var total = await Repository.GetOrganizationsCount(
                filter: _filter);
            var organizations = await Repository.GetOrganizations(
                filter: _filter,
                startIndex: request.StartIndex, 
                takeCount: Math.Min(request.Count, total - request.StartIndex));

            await TotalChanged.InvokeAsync(total);

            return new ItemsProviderResult<OrganizationDTO>(organizations, total);
        }
        finally
        {
            _isLoading = false;
        }

    }
}