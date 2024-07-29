using C8S.AdminApp.Services;
using C8S.Common;
using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Home : BaseRazorPage
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public SelfService SelfService { get; set; } = default!;

    private string _username = SharedConstants.Display.NotSet;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _username = SelfService.Username;
    }
}