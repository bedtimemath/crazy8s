using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateClient
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("phone_number")]
    public string? Phone { get; set; } = null;

    [JsonPropertyName("email")]
    public string? Email { get; set; } = null;
}