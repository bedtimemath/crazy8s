using Blazr.RenderState;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public partial class SidebarMenu: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenu> Logger { get; set; } = null!;

    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;
    #endregion
}