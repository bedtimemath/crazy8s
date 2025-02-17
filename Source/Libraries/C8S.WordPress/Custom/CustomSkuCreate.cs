using C8S.WordPress.Abstractions;
using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomSkuCreate
{
    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; set; } = null!;

    [JsonProperty(PropertyName = "type")] 
    public string Type { get; set; } = WordPressConstants.CustomPostTypes.Sku;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; } = null!;

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = null!;

    [JsonProperty(PropertyName = "acf")]
    public CustomSkuACF ACF { get; set; } = null!;
}