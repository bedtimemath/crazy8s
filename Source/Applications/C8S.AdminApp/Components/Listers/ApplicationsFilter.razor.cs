using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Listers;

public partial class ApplicationsFilter : BaseRazorComponent
{
    [Inject]
    public ILogger<ApplicationsFilter> Logger { get; set; } = default!;

    [Parameter]
    public ApplicationFilter Filter { get; set; } = new();

    [Parameter]
    public EventCallback<ApplicationFilter> FilterChanged { get; set; }
}
