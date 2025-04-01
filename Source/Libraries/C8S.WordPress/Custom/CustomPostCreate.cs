using C8S.WordPress.Abstractions;
using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomPostCreate
{
    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; set; } = null!;

    [JsonProperty(PropertyName = "type")] 
    public string Type { get; set; } = WordPressConstants.CustomPostTypes.KitPage;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; } = null!;

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = null!;

    [JsonProperty(PropertyName = "acf")]
    public CustomPostACF ACF { get; set; } = null!;
}