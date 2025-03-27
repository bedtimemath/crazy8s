using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

public record SkuDTO
{
    [JsonPropertyName("sku_id")]
    public int SkuId { get; init; }

    [JsonPropertyName("fulco_id")]
    public string FulcoId { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; init; } = default!;

    [JsonPropertyName("year")]
    public string Year { get; init; } = null!;

    [JsonPropertyName("season")]
    public int Season { get; init; }

    [JsonPropertyName("age_level")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; init; } = default!;

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("club_key")]
    public string ClubKey { get; init; } = null!;
}