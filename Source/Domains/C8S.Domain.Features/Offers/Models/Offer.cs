using C8S.Domain.Enums;

namespace C8S.Domain.Features.Offers.Models;

public record Offer
{
    public int OfferId { get; init; }
    public string FulcoId { get; init; } = null!;
    public string Title { get; init; } = null!;
    public OfferStatus Status { get; init; }
    public string Year { get; init; } = null!;
    public int Season { get; init; }
    public string? Version { get; init; }
    public string? Description { get; init; }
}