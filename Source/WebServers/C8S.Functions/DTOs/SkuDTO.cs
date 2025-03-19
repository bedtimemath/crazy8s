using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using C8S.Domain.Features.Clubs.Models;

namespace C8S.Functions.DTOs;

public record SkuDTO
{
    [JsonPropertyName("sku_id")]
    public int SkuId { get; init; }

    [JsonPropertyName("key")]
    public string Key { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; init; } = default!;

    [JsonPropertyName("year")]
    public string? Year { get; init; }

    [JsonPropertyName("season")]
    public int? Season { get; init; }

    [JsonPropertyName("age_level")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel? AgeLevel { get; init; }

    [JsonPropertyName("club_size")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize? ClubSize { get; init; }
}