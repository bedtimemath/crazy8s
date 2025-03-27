using C8S.Domain.Enums;

namespace C8S.WordPress.Abstractions.Models;

public record WPSkuCreate
{
    public string Slug { get; init; } = null!;
    public string Title { get; init; } = null!;
    public OfferStatus Status { get; init; }
    public WPSkuProperties Properties { get; init; } = null!;
}