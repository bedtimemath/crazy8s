using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

public class FullSlateOpeningsList
{
    [JsonPropertyName("services")]
    public List<FullSlateOffering> Offerings { get; set; } = new();

    [JsonPropertyName("openings")]
    public List<DateTimeOffset> Openings { get; set; } = new();
}