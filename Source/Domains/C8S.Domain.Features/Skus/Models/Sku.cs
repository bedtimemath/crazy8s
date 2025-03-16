using C8S.Domain.Enums;

namespace C8S.Domain.Features.Skus.Models;

public record Sku
{
    public int SkuId { get; init; }
    public string Key { get; init; } = null!;
    public string Name { get; init; } = null!;
    public SkuStatus Status { get; init; }
    public string? Year { get; init; }
    public int? Season { get; init; }
    public AgeLevel? AgeLevel { get; init; }
    public ClubSize? ClubSize { get; init; }
}