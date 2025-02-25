using System.Text.Json.Serialization;

namespace C8S.Fulco.Abstractions.Models;

public record FulcoOrderClassification
{
    [JsonPropertyName("CustomerCode")]
    public string? CustomerCode { get; init; }
    
    [JsonPropertyName("Store")]
    public string? Store { get; init; }
    
    [JsonPropertyName("Department")]
    public string? Department { get; init; }
    
    [JsonPropertyName("Vendor")]
    public string? Vendor { get; init; }
    
    [JsonPropertyName("OrderEntryView")]
    public string OrderEntryView { get; init; } = null!;
    
    [JsonPropertyName("OrderProcessingStream")]
    public string OrderProcessingStream { get; init; } = null!;
    
    [JsonPropertyName("ProjectID")]
    public string ProjectId { get; init; } = null!;
    
    [JsonPropertyName("ProjectDescription")]
    public string ProjectDescription { get; init; } = null!;
    
    [JsonPropertyName("CampaignID")]
    public string? CampaignId { get; init; }
    
    [JsonPropertyName("DistributionID")]
    public string? DistributionId { get; init; }
    
    [JsonPropertyName("DistributionCenter")]
    public string? DistributionCenter { get; init; }
    
    [JsonPropertyName("RushOrder")]
    public bool RushOrder { get; init; }
    
    [JsonPropertyName("ResponseMedia")]
    public string? ResponseMedia { get; init; }
    
    [JsonPropertyName("SourceType")]
    public string? SourceType { get; init; }
    
    [JsonPropertyName("Source")]
    public string? Source { get; init; }
    
    [JsonPropertyName("SourceDetail")]
    public string? SourceDetail { get; init; }
    
    [JsonPropertyName("SourceDetailIssueDate")]
    public DateTime? SourceDetailIssueDate { get; init; }
}