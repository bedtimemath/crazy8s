using C8S.Domain.Features.Requests.Lists;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Displayers;

public partial class RequestDisplayer: BaseRazorComponent
{
    [Parameter]
    public RequestListItem Request { get; set; } = null!;
}