using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoOrder
{
    [JsonPropertyName("ID")]
    public string Id { get; init; } = null!;
    
    [JsonPropertyName("PurchaseOrder")]
    public string? PurchaseOrder { get; init; }
    
    [JsonPropertyName("ReferenceNumber")]
    public string? ReferenceNumber { get; init; }
    
    [JsonPropertyName("Comments")]
    public string? Comments { get; init; }
    
    [JsonPropertyName("CurrentOrderStatus")]
    public string CurrentOrderStatus { get; init; } = null!;
    
    [JsonPropertyName("OrderDates")]
    public FulcoOrderDates OrderDates { get; init; } = null!;
    
    [JsonPropertyName("OrderClassification")]
    public FulcoOrderClassification OrderClassification { get; init; } = null!;
    
    [JsonPropertyName("OrderedBy")]
    public FulcoContact OrderedBy { get; init; } = null!;
    
    [JsonPropertyName("Shipments")]
    public List<FulcoShipment> Shipments { get; init; } = null!;
}