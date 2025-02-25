using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoOption
{
    [JsonPropertyName("SizeCode")]
    public string? SizeCode { get; init; }
    
    [JsonPropertyName("SizeDescription")]
    public string? SizeDescription { get; init; }
    
    [JsonPropertyName("ColorCode")]
    public string? ColorCode { get; init; }
    
    [JsonPropertyName("ColorDescription")]
    public string? ColorDescription { get; init; }
}
