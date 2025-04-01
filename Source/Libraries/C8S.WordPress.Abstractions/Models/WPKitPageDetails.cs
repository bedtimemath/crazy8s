namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageDetails
{
    public int Id { get; init; }
    public string Slug { get; init; } = null!;
    public string Type { get; init; } = null!;
    public string Status { get; init; } = null!;

    public string Title { get; init; } = null!;

    public WPKitPageProperties? Properties { get; init; } = null!;
}