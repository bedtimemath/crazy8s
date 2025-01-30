using C8S.Domain.Features.Requests.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;
using SC.Messaging.Abstractions.Interfaces;


namespace C8S.AdminApp.Client.UI.Requests;

public partial class RequestListItemDisplayer : BaseRazorComponent
{
    [Inject]
    public ICQRSService CQRSService { get; set; } = null!;

    [Parameter]
    public RequestListItem Request { get; set; } = null!;
}