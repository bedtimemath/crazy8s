using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoOrderDates
{
    [JsonPropertyName("UTCOrderDate")]
    public DateTime UtcOrderDate { get; init; }
    
    [JsonPropertyName("ServerDateCaptured")]
    public DateTime? ServerDateCaptured { get; init; }
    
    [JsonPropertyName("NeededByDate")]
    public DateTime? NeededByDate { get; init; }
}