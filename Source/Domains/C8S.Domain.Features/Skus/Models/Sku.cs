using C8S.Domain.Enums;

namespace C8S.Domain.Features.Skus.Models;

public record Sku
{
    public int SkuId { get; init; }
    public string FulcoId { get; init; } = null!;
    public string Name { get; init; } = null!;
    public SkuStatus Status { get; init; }
    public string Year { get; init; } = null!;
    public int Season { get; init; }
    public AgeLevel AgeLevel { get; init; }
    public string? Version { get; init; }
    public string ClubKey { get; init; }
}