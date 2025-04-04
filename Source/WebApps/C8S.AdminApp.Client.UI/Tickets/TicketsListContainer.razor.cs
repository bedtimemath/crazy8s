using C8S.AdminApp.Client.Services.Coordinators.Tickets;
using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Tickets.Models;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.UI.Tickets;

public sealed partial class TicketsListContainer : BaseRazorListContainer<TicketsListCoordinator, TicketListItem>
{
    #region Injected Properties
    //[Inject]
    //public ILogger<TicketsList> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public override TicketsListCoordinator Coordinator { get; set; } = null!;
    #endregion

}