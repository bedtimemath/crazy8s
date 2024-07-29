using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Applications : BaseRazorPage
{
    [Inject]
    public ILogger<Applications> Logger { get; set; } = default!;
}