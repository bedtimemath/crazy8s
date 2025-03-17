using C8S.Domain.Features.Clubs.Models;

namespace C8S.Domain.Features.ClubPersons.Models;

public record ClubPersonWithOrders
{
    public int ClubPersonId { get; set; }
    public bool IsPrimary { get; set; }
    public ClubWithOrders Club { get; set; } = null!;
}