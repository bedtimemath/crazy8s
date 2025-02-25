using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoContact
{
    [JsonPropertyName("Name")]
    public string Name { get; init; } = null!;
    
    [JsonPropertyName("Company")]
    public string Company { get; init; } = null!;
    
    [JsonPropertyName("Title")]
    public string? Title { get; init; }
    
    [JsonPropertyName("Address1")]
    public string Address1 { get; init; } = null!;
    
    [JsonPropertyName("Address2")]
    public string Address2 { get; init; } = null!;
    
    [JsonPropertyName("Address3")]
    public string? Address3 { get; init; }
    
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
    
    [JsonPropertyName("Fax")]
    public string? Fax { get; init; }
    
    [JsonPropertyName("Email")]
    public string Email { get; init; } = null!;
    
    [JsonPropertyName("UniqueIdentifier")]
    public string? UniqueIdentifier { get; init; }
    
    [JsonPropertyName("TaxExemptFlag")]
    public string TaxExemptFlag { get; init; } = null!;
    
    [JsonPropertyName("TaxExemptID")]
    public string? TaxExemptId { get; init; }
    
    [JsonPropertyName("TaxExemptApprovedFlag")]
    public string TaxExemptApprovedFlag { get; init; } = null!;
    
    [JsonPropertyName("CommercialFlag")]
    public string CommercialFlag { get; init; } = null!;
}