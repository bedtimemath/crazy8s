using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.Filters;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Listers;

public partial class CoachesFilterDescription: BaseRazorComponent
{
    [Parameter]
    public CoachFilter Filter { get; set; } = default!;

    [Parameter]
    public int Total { get; set; } = default(int);
}