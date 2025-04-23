using C8S.Domain.Enums;
using C8S.Domain.Features.Tickets.Models;
using SC.Common.Helpers.CQRS.Base;

namespace C8S.Domain.Features.Tickets.Queries;

public record TicketsListQuery: BaseListQuery<TicketListItem>
{
    public DateOnly? SubmittedAfter { get; init; }
    public DateOnly? SubmittedBefore { get; init; }
    public bool? HasCoachCall { get; init; }
    public IList<TicketStatus>? Statuses { get; init; }
}