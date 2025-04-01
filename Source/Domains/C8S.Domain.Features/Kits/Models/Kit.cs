using C8S.Domain.Enums;
using C8S.Domain.Features.Offers.Models;

namespace C8S.Domain.Features.Kits.Models;

public record Kit
{
    public int KitId { get; init; }
    public string Key { get; init; } = null!;
    public KitStatus Status { get; init; }
    public string Year { get; init; } = null!;
    public int Season { get; init; }
    public AgeLevel AgeLevel { get; init; }
    public string? Version { get; init; }
    public string? Comments { get; init; }

    public Offer? Offer { get; init; }
}