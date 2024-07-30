using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Listers;

public partial class CoachesFilter : BaseRazorComponent
{
    [Inject]
    public ILogger<CoachesFilter> Logger { get; set; } = default!;

    [Parameter]
    public CoachFilter Filter { get; set; } = default!;

    [Parameter]
    public EventCallback<CoachFilter> FilterChanged { get; set; }
}
