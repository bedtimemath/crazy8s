using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

public record ClubDTO
{
    [JsonPropertyName("club_id")]
    public int ClubId { get; init; }

    [JsonPropertyName("club_status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubStatus ClubStatus { get; init; } = default!;

    [JsonPropertyName("kit_status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KitStatus KitStatus { get; init; } = default!;

    [JsonPropertyName("key")]
    public string Key { get; init; } = null!;

    [JsonPropertyName("year")]
    public string Year { get; init; } = null!;

    [JsonPropertyName("season")]
    public int Season { get; init; }

    [JsonPropertyName("age_level")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("starts_on")]
    public DateOnly? StartsOn { get; init; }

    [JsonPropertyName("orders")]
    public List<OrderDTO> Orders { get; init; } = [];
}