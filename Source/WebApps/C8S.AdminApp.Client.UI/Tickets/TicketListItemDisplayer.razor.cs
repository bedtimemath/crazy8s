using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Tickets.Models;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.UI.Tickets;

public sealed partial class TicketListItemDisplayer: BaseRazorListItemDisplayer
{
    [Parameter]
    public TicketListItem Ticket { get; set; } = null!;
}