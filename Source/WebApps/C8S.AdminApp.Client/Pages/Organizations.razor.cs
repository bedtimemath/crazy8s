using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public partial class Organizations : BaseRazorPage
{
    [Inject]
    public ILogger<Organizations> Logger { get; set; } = default!;
}