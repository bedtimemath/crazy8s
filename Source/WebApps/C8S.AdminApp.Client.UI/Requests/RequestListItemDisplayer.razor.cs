using C8S.Domain.Features.Requests.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Requests;

public partial class RequestListItemDisplayer : BaseRazorComponent
{
    [Inject]
    public ISender Sender { get; set; } = null!;

    [Parameter]
    public RequestListItem Request { get; set; } = null!;
}