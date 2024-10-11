using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateErrorResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;

    [JsonPropertyName("errors")]
    public List<FullSlateError> Errors { get; set; } = new();

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? Extras { get; set; } = null;
}
