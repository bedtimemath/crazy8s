using MediatR;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public partial class RequestDetailsPage: BaseRazorPage
{
    [Inject]
    public ISender Sender { get; set; } = null!;
    
    [Parameter]
    public int RequestId { get; set; }
}