using Newtonsoft.Json;

namespace C8S.WordPress.Custom;

internal class CustomPostACF
{
    [JsonProperty(PropertyName = "key")]
    public string Key { get; set; } = null!;
    
    [JsonProperty(PropertyName = "year")]
    public string Year { get; set; } = null!;

    [JsonProperty(PropertyName = "season")]
    public int Season { get; set; }

    [JsonProperty(PropertyName = "age_level")]
    public string AgeLevel { get; set; } = null!;

    [JsonProperty(PropertyName = "version")]
    public string? Version { get; set; } = null!;
}