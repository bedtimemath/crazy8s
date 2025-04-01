using C8S.Domain.Features.Offers.Models;

namespace C8S.Domain.Features.OrderOffers.Models;

public record OrderOffer
{
    public int OrderOfferId { get; init; }
    public int Ordinal { get; init; }
    public int Quantity { get; init; }

    public Offer Offer { get; init; } = null!;
}