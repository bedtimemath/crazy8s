using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoReturnAddress
{
    [JsonPropertyName("Name")]
    public string? Name { get; init; }
    
    [JsonPropertyName("Company")]
    public string Company { get; init; } = null!;
    
    [JsonPropertyName("Address1")]
    public string Address1 { get; init; } = null!;
    
    [JsonPropertyName("Address2")]
    public string Address2 { get; init; } = null!;
    
    [JsonPropertyName("Address3")]
    public string Address3 { get; init; } = null!;
    
    [JsonPropertyName("City")]
    public string City { get; init; } = null!;
    
    [JsonPropertyName("State")]
    public string State { get; init; } = null!;
    
    [JsonPropertyName("PostalCode")]
    public string PostalCode { get; init; } = null!;
    
    [JsonPropertyName("Country")]
    public string Country { get; init; } = null!;
    
    [JsonPropertyName("Phone")]
    public string Phone { get; init; } = null!;
}