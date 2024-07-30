using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Listers;

public partial class OrganizationsFilterDescription: BaseRazorComponent
{
    [Parameter]
    public OrganizationFilter Filter { get; set; } = default!;

    [Parameter]
    public int Total { get; set; } = default(int);
}