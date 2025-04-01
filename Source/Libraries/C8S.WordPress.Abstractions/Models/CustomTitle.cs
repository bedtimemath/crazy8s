using Newtonsoft.Json;

namespace C8S.WordPress.Abstractions.Models;

public class CustomTitle
{
    [JsonProperty(PropertyName = "rendered")]
    public string? Rendered { get; set; }
}