using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoDimensions
{
    [JsonPropertyName("Measurement")]
    public string Measurement { get; init; } = null!;
    
    [JsonPropertyName("Length")]
    public double? Length { get; init; }
    
    [JsonPropertyName("Width")]
    public double? Width { get; init; }
    
    [JsonPropertyName("Height")]
    public double? Height { get; init; }
}