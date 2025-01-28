using C8S.AdminApp.Client.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Navigation;

public sealed partial class NavigationMenu: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<NavigationMenu> Logger { get; set; } = null!;

    [Inject]
    public NavigationService NavigationService { get; set; } = null!;
    #endregion
}