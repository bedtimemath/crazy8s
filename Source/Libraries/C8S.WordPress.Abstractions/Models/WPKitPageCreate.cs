using Newtonsoft.Json;

namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageCreate
{
    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; init; } = null!;
    
    [JsonProperty(PropertyName = "title")]
    public string Title { get; init; } = null!;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; init; } = null!;
    
    [JsonProperty(PropertyName = "acf")]
    public WPKitPageProperties? Properties { get; init; } = null!;
}