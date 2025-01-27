using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFilter: BaseRazorComponent
{
    [Inject]
    public ILogger<RequestsFilter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListCoordinator Coordinator { get; set; } = null!;
}
