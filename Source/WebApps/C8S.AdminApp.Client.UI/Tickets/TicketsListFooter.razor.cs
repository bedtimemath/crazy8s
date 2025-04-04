using C8S.AdminApp.Client.Services.Coordinators.Tickets;
using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Tickets.Models;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.UI.Tickets;

public sealed partial class TicketsListFooter: BaseRazorListFooter<TicketsListCoordinator, TicketListItem>
{
    #region Component Parameters
    [Parameter]
    public override TicketsListCoordinator Coordinator { get; set; } = null!;
    #endregion
}