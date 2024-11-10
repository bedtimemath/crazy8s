using Blazr.RenderState;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.Components.Base;

public abstract class BaseRenderStateComponent : BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = default!;
    #endregion

    #region Protected Properties
    protected bool IsPreRender => RenderStateService.IsPreRender;
    #endregion

}