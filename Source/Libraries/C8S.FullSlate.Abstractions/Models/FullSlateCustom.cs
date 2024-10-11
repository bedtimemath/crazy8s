using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlateCustom
{
    //   label	string
    //   example: Vehicle Brand
    //   Label/Name of the custom field
    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    //   value
    [JsonPropertyName("value")]
    public object Value { get; set; } = default!;
}