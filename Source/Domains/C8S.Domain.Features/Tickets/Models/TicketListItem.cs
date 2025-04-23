namespace C8S.Domain.Features.Tickets.Models;

public record TicketListItem: Ticket
{
    public string? PlaceName { get; init; }
    public string? PlaceTypeString { get; init; }
    public string? PlaceFullAddress { get; init; }

    public string? PrimaryFullName { get; init; }
    public string? PrimaryEmail { get; init; }

    public DateTimeOffset? RequestAppointmentStartsOn { get; init; }
    public string? RequestClubsRequested { get; init; }
    public DateTimeOffset? RequestSubmittedOn { get; init; }
}