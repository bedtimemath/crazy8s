using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Interactions;

[Serializable]
public class FullSlateErrorResponse
{
    [JsonPropertyName("failure")]
    public bool Failure { get; set; } = true;

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; } = null;

    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; } = null;

    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; } = null;

    [JsonPropertyName("requestTime")]
    public DateTimeOffset? RequestTime { get; set; } = null;

    // not from FullSlate; used in our own exceptions
    [JsonPropertyName("details")]
    public JsonElement? Details { get; set; } = default!;
}
