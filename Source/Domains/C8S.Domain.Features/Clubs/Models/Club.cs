using C8S.Domain.Enums;

namespace C8S.Domain.Features.Clubs.Models;

public record Club
{
    public int ClubId { get; init; }
    public ClubStatus Status { get; init; }
    public int? Season { get; init; }
    public AgeLevel? AgeLevel { get; init; }
    public DateOnly? StartsOn { get; init; }
}