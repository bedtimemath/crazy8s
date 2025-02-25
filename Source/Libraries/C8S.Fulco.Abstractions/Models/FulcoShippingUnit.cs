using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoShippingUnit
{
    [JsonPropertyName("ShipFrom")]
    public FulcoShipFrom ShipFrom { get; init; } = null!;
    
    [JsonPropertyName("RequestedShippingOption")]
    public string RequestedShippingOption { get; init; } = null!;
    
    [JsonPropertyName("RequestedFreightCarrier")]
    public string RequestedFreightCarrier { get; init; } = null!;
    
    [JsonPropertyName("RequestedFreightService")]
    public string RequestedFreightService { get; init; } = null!;
    
    [JsonPropertyName("RequestedFreightCode")]
    public string? RequestedFreightCode { get; init; }
    
    [JsonPropertyName("Comments")]
    public string? Comments { get; init; }
    
    [JsonPropertyName("SignatureRequirement")]
    public string? SignatureRequirement { get; init; }
    
    [JsonPropertyName("TotalWeight")]
    public double TotalWeight { get; init; }
    
    [JsonPropertyName("TotalWeightType")]
    public string TotalWeightType { get; init; } = null!;
    
    [JsonPropertyName("PackageType")]
    public FulcoPackageType PackageType { get; init; } = null!;
    
    [JsonPropertyName("Items")]
    public List<FulcoItem> Items { get; init; } = null!;
    
    [JsonPropertyName("FreightAccount")]
    public FulcoFreightAccount FreightAccount { get; init; } = null!;
}
