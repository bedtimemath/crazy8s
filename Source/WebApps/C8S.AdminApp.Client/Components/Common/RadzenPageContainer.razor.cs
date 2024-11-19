using Blazr.RenderState;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Components.Common;

public partial class RadzenPageContainer : ComponentBase
{
    #region Injected Properties
    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = default!;
    #endregion

    #region Page Parameters
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public bool HideDuringPreRender { get; set; } = false;
    #endregion

    #region Protected Properties
    protected bool IsPreRender => RenderStateService.IsPreRender;
    #endregion

    #region Page LifeCycle
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion
}