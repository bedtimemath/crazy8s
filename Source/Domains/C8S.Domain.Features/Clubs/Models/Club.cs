using C8S.Domain.Enums;
using C8S.Domain.Features.Kits.Models;

namespace C8S.Domain.Features.Clubs.Models;

public record Club
{
    public int ClubId { get; init; }
    public ClubStatus Status { get; init; }
    public DateOnly? StartsOn { get; init; }

    public Kit Kit { get; init; } = null!;
}