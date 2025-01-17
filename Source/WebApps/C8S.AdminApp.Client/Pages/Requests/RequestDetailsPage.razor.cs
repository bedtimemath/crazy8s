using Blazr.RenderState;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public sealed partial class RequestDetailsPage : 
    BaseOwningRazorPage<RequestDetailsCoordinator>
{
    [Inject]
    public ILogger<RequestDetailsPage> Logger { get; set; } = null!;
    
    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = null!;
    
    [Parameter]
    public int RequestId { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        if (!RenderStateService.IsPreRender)
            Service.SetDetailsId(RequestId);
    }
}