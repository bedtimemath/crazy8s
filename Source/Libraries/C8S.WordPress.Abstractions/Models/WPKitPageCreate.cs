using C8S.Domain.Enums;

namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageCreate
{
    public string Slug { get; init; } = null!;
    public string Title { get; init; } = null!;
    public WPPageStatus Status { get; init; }
    public WPKitPageProperties Properties { get; init; } = null!;
}