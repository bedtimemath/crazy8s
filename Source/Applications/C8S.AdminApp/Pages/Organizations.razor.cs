using C8S.AdminApp.Components.Listers;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Organizations : BaseRazorPage
{
    [Inject]
    public ILogger<Organizations> Logger { get; set; } = default!;

    private OrganizationsLister _organizationsLister = default!;

    private OrganizationFilter _filter = new();
    private int _total = default(int);

    private async Task HandleFilterChanged(OrganizationFilter filter)
    {
        Logger.LogInformation("HandleFilterChanged: {@Filter}", filter);

        _filter = filter;
        await _organizationsLister.SetFilter(filter);
    }

    private void HandleTotalChanged(int count)
    {
        Logger.LogInformation("HandleCountChanged: {@Count}", count);

        _total = count;
        StateHasChanged();
    }
}