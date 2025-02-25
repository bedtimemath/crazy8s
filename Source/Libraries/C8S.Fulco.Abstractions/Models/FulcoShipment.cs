using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoShipment
{
    [JsonPropertyName("ShipTo")]
    public FulcoShipTo ShipTo { get; init; } = null!;
    
    [JsonPropertyName("ShippingUnits")]
    public List<FulcoShippingUnit> ShippingUnits { get; init; } = null!;
    
    [JsonPropertyName("ReturnAddress")]
    public FulcoReturnAddress ReturnAddress { get; init; } = null!;
}
