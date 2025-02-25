using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoProduct
{
    [JsonPropertyName("ID")]
    public string ID { get; init; } = null!;
    
    [JsonPropertyName("Description")]
    public string Description { get; init; } = null!;
    
    [JsonPropertyName("Weight")]
    public double? Weight { get; init; }
    
    [JsonPropertyName("QuantityOrdered")]
    public int QuantityOrdered { get; init; }
    
    [JsonPropertyName("WeightType")]
    public string WeightType { get; init; } = null!;
    
    [JsonPropertyName("Options")]
    public List<FulcoOption> Options { get; init; } = null!;
    
    [JsonPropertyName("Transportation")]
    public FulcoTransportation Transportation { get; init; } = null!;
}