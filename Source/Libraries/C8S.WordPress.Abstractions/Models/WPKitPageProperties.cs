using C8S.Domain.Enums;

namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageProperties
{
    public string Key { get; set; } = null!;

    public string Year { get; set; } = null!;

    public int Season { get; set; }

    public AgeLevel AgeLevel { get; set; }

    public string? Version { get; set; } = null!;
}