using C8S.Domain.Features.Orders.Models;

namespace C8S.Domain.Features.Clubs.Models;

public record ClubWithOrders: Club
{
    public List<Order> Orders { get; init; } = [];
}