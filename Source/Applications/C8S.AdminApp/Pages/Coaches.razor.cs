using C8S.AdminApp.Components.Listers;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Coaches : BaseRazorPage
{
    [Inject]
    public ILogger<Coaches> Logger { get; set; } = default!;

    private CoachesLister _coachesLister = default!;

    private CoachFilter _filter = new();
    private int _total = default(int);

    private async Task HandleFilterChanged(CoachFilter filter)
    {
        Logger.LogInformation("HandleFilterChanged: {@Filter}", filter);

        _filter = filter;
        await _coachesLister.SetFilter(filter);
    }

    private void HandleTotalChanged(int count)
    {
        Logger.LogInformation("HandleCountChanged: {@Count}", count);

        _total = count;
        StateHasChanged();
    }
}