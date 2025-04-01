using Newtonsoft.Json;

namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageDetails
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; init; }

    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; init; } = null!;

    [JsonProperty(PropertyName = "type")]
    public string Type { get; init; } = null!;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; init; } = null!;

    [JsonProperty(PropertyName = "title")]
    public CustomTitle Title { get; init; } = null!;

    [JsonProperty(PropertyName = "acf")]
    public WPKitPageProperties? Properties { get; init; } = null!;
}