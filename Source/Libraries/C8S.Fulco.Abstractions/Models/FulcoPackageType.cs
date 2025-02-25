using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoPackageType
{
    [JsonPropertyName("Description")]
    public string? Description { get; init; }
    
    [JsonPropertyName("Weight")]
    public double? Weight { get; init; }
    
    [JsonPropertyName("WeightType")]
    public string? WeightType { get; init; }
    
    [JsonPropertyName("Dimensions")]
    public FulcoDimensions Dimensions { get; init; } = null!;
    
    [JsonPropertyName("PackageCarrierID")]
    public string? PackageCarrierId { get; init; }
}