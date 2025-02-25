using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoPricing
{
    [JsonPropertyName("Price")]
    public double? Price { get; init; }
    
    [JsonPropertyName("PriceType")]
    public string PriceType { get; init; } = null!;
}