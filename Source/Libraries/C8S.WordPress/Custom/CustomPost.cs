using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomPost
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; set; } = null!;

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; } = null!;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; } = null!;

    [JsonProperty(PropertyName = "title")]
    public CustomTitle Title { get; set; } = new();

    [JsonProperty(PropertyName = "acf")]
    public CustomPostACF ACF { get; set; } = new();
}