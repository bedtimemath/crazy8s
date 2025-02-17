using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomSkuACF
{
    [JsonProperty(PropertyName = "sku_identifier")]
    public string SkuIdentifier { get; set; } = null!;

    [JsonProperty(PropertyName = "season")]
    public int Season { get; set; }

    [JsonProperty(PropertyName = "age_range")]
    public string AgeRange { get; set; } = null!;
}