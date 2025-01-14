using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Requests.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFilter: BaseClientComponent
{
    
    [Inject]
    public ILogger<RequestsFilter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListController Controller { get; set; } = null!;
}
