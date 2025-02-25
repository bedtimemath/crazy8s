using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoTransportation
{
    [JsonPropertyName("CountryOfOrigin")]
    public string? CountryOfOrigin { get; init; }
    
    [JsonPropertyName("TariffCode")]
    public string? TariffCode { get; init; }
    
    [JsonPropertyName("CustomsValue")]
    public double? CustomsValue { get; init; }
    
    [JsonPropertyName("InsuranceValue")]
    public double? InsuranceValue { get; init; }
    
    [JsonPropertyName("CommodityDescription")]
    public string? CommodityDescription { get; init; }
    
    [JsonPropertyName("NMFCNumber")]
    public string? NmfcNumber { get; init; }
    
    [JsonPropertyName("FreightClass")]
    public string? FreightClass { get; init; }
}