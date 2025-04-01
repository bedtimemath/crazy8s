using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

public record OfferDTO
{
    [JsonPropertyName("offer_id")]
    public int OfferId { get; init; }

    [JsonPropertyName("fulco_id")]
    public string FulcoId { get; init; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OfferStatus Status { get; init; } = default!;

    [JsonPropertyName("year")]
    public string Year { get; init; } = null!;

    [JsonPropertyName("season")]
    public int Season { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}