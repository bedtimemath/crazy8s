using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Coaches : BaseRazorPage
{
    [Inject]
    public ILogger<Coaches> Logger { get; set; } = default!;
}