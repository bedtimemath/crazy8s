using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

public record ClubDTO
{
    [JsonPropertyName("club_id")]
    public int ClubId { get; init; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubStatus Status { get; init; } = default!;

    [JsonPropertyName("year")]
    public string? Year { get; init; }

    [JsonPropertyName("season")]
    public string? Season { get; init; }

    [JsonPropertyName("age_level")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel? AgeLevel { get; init; }

    [JsonPropertyName("club_size")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize? ClubSize { get; init; }

    [JsonPropertyName("starts_on")]
    public DateOnly? StartsOn { get; init; }

    [JsonPropertyName("orders")]
    public List<OrderDTO> Orders { get; init; } = [];
}