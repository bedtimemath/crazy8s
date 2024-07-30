using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Listers;

public partial class OrganizationsFilter : BaseRazorComponent
{
    [Inject]
    public ILogger<OrganizationsFilter> Logger { get; set; } = default!;

    [Parameter]
    public OrganizationFilter Filter { get; set; } = default!;

    [Parameter]
    public EventCallback<OrganizationFilter> FilterChanged { get; set; }
}
