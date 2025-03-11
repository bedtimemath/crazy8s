namespace C8S.WordPress.Abstractions.Models;

public record WPRoleDetails
{
    public string Slug { get; init; } = null!;
    public string Display { get; init; } = null!;
    public List<string> Capabilities { get; init; } = [];
}