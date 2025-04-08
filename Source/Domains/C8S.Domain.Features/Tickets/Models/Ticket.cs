using C8S.Domain.Enums;

namespace C8S.Domain.Features.Tickets.Models;

public record Ticket
{
    public int TicketId { get; init; }
    public TicketStatus Status { get; init; }
    public int? PlaceId { get; init; }
    public int? RequestId { get; init; }
    public int? InvoiceId { get; init; }
    
    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
}