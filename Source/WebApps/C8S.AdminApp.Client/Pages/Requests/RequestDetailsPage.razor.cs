using Blazr.RenderState;
using C8S.AdminApp.Client.Services.Controllers.Requests;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public sealed partial class RequestDetailsPage : 
    BaseOwningRazorPage<RequestDetailsController>
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
        
        Logger.LogInformation("IsPreRender = {IsPreRender}", RenderStateService.IsPreRender);
        if (!RenderStateService.IsPreRender)
            Service.SetDetailsId(RequestId);
    }
}