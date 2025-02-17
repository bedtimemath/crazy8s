using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomTitle
{
    [JsonProperty(PropertyName = "rendered")]
    public string? Rendered { get; set; }
}