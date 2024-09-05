using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateOffering // "Service" in FullSlate terms
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
}