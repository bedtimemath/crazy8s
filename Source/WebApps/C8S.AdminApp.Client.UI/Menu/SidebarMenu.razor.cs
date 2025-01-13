using Blazr.RenderState;
using C8S.AdminApp.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarMenu: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenu> Logger { get; set; } = null!;

    [Inject]
    public IPagesService PagesService { get; set; } = null!;
    #endregion
}