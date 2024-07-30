using C8S.AdminApp.Components.Listers;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Applications : BaseRazorPage
{
    [Inject]
    public ILogger<Applications> Logger { get; set; } = default!;

    private ApplicationsLister _applicationsLister = default!;

    private async Task HandleFilterChanged(ApplicationFilter filter)
    {
        Logger.LogInformation("HandleFilterChanged: {@Filter}", filter);
        await _applicationsLister.SetFilter(filter);
    }
}