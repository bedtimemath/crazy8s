using System.Text.Json.Serialization;

namespace C8S.Functions.DTOs;

[Serializable]
public record PersonDTO
{
    [JsonPropertyName("person_id")]
    public int PersonId { get; init; }

    [JsonPropertyName("last_name")]
    public string LastName { get; init; } = null!;

    [JsonPropertyName("first_name")]
    public string? FirstName { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; init; }
}