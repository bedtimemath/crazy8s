namespace C8S.WordPress.Abstractions.Models;

public record WPRoleCreate
{
    public string Slug { get; init; } = null!;
    public string Display { get; init; } = null!;
    public object? Capabilities { get; init; }
}