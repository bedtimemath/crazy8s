using C8S.Domain.Enums;

namespace C8S.WordPress.Abstractions.Models;

public record WPSkuProperties
{
    public string SkuIdentifier { get; set; } = null!;

    public int Season { get; set; }

    public AgeLevel AgeLevel { get; set; }
}