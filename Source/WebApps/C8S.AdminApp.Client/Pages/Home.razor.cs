using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public sealed partial class Home : BaseRazorPage
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = null!;
}