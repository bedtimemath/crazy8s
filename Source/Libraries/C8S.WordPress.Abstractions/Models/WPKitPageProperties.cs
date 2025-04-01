using C8S.Domain.Enums;
using Newtonsoft.Json;

namespace C8S.WordPress.Abstractions.Models;

public record WPKitPageProperties
{
    [JsonProperty(PropertyName = "key")]
    public string? Key { get; set; }
    
    [JsonProperty(PropertyName = "year")]
    public string? Year { get; set; }

    [JsonProperty(PropertyName = "season")]
    public int? Season { get; set; }

    [JsonProperty(PropertyName = "age_level")]
    public string? AgeLevel { get; set; }
    
    [JsonProperty(PropertyName = "version")]
    public string? Version { get; set; }
}