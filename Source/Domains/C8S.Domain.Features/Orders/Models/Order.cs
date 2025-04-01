using C8S.Domain.Enums;
using C8S.Domain.Features.OrderOffers.Models;

namespace C8S.Domain.Features.Orders.Models;

public record Order
{
    public int OrderId { get; init; }
    public int Number { get; init; }
    public OrderStatus Status { get; init; }
    public DateTimeOffset OrderedOn { get; init; }
    public List<OrderOffer> OrderOffers { get; init; } = [];
}