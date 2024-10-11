using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateError
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;

    [JsonPropertyName("field_name")]
    public string FieldName { get; set; } = default!;

    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("example")]
    public string Example { get; set; } = default!;

    [JsonPropertyName("in")]
    public string In { get; set; } = default!;
}