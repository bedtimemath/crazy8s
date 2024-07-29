using C8S.AdminApp.Services;
using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Site;

public partial class HeaderProfile: BaseRazorComponent
{
    [Inject]
    public SelfService SelfService { get; set; } = default!;
}