using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoFreightAccount
{
    [JsonPropertyName("ThirdPartyBillingType")]
    public string ThirdPartyBillingType { get; init; } = null!;
    
    [JsonPropertyName("AccountNumber")]
    public string? AccountNumber { get; init; }
    
    [JsonPropertyName("Name")]
    public string Name { get; init; } = null!;
    
    [JsonPropertyName("Company")]
    public string? Company { get; init; }
    
    [JsonPropertyName("Address1")]
    public string? Address1 { get; init; }
    
    [JsonPropertyName("Address2")]
    public string? Address2 { get; init; }
    
    [JsonPropertyName("Address3")]
    public string? Address3 { get; init; }
    
    [JsonPropertyName("City")]
    public string? City { get; init; }
    
    [JsonPropertyName("State")]
    public string? State { get; init; }
    
    [JsonPropertyName("PostalCode")]
    public string? PostalCode { get; init; }
    
    [JsonPropertyName("Country")]
    public string? Country { get; init; }
    
    [JsonPropertyName("Phone")]
    public string? Phone { get; init; }
}