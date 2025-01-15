using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFilter: BaseClientComponent
{
    [Inject]
    public ILogger<RequestsFilter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListCoordinator Coordinator { get; set; } = null!;
}
