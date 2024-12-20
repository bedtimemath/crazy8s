using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Interactions;

[Serializable]
public class ServiceError
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; } = null;

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; } = null;

    [JsonPropertyName("fieldName")]
    public string? FieldName { get; set; } = null;

    [JsonPropertyName("example")]
    public string? Example { get; set; } = null;

    [JsonPropertyName("details")]
    public string? Details { get; set; } = null;
}
