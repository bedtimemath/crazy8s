using Blazr.RenderState;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public partial class RequestDetailsPage: BaseRazorPage
{
    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = null!;
    
    [Parameter]
    public int RequestId { get; set; }
}