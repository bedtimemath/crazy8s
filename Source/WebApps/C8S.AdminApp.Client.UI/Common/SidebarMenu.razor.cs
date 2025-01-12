using Blazr.RenderState;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Common;

public partial class SidebarMenu: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarMenu> Logger { get; set; } = default!;

    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;
    #endregion
}