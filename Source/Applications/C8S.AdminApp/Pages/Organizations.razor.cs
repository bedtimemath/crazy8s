using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Organizations : BaseRazorPage
{
    [Inject]
    public ILogger<Organizations> Logger { get; set; } = default!;
}