using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoItem
{
    [JsonPropertyName("LineNumber")]
    public int LineNumber { get; init; }
    
    [JsonPropertyName("ID")]
    public string ID { get; init; } = null!;
    
    [JsonPropertyName("Title")]
    public string Title { get; init; } = null!;
    
    [JsonPropertyName("QuantityOrdered")]
    public int QuantityOrdered { get; init; }
    
    [JsonPropertyName("Pricing")]
    public FulcoPricing Pricing { get; init; } = null!;
    
    [JsonPropertyName("Products")]
    public List<FulcoProduct> Products { get; init; } = null!;
}