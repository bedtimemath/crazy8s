using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateBadRequestResponse
{
   /*
    {
        "failure": true,
        "errorMessage": "Sorry, the selected time is no longer available. Please send the requestId to the support team.",
        "requestId": "cbbdb40b-8114-40c7-a078-160a78b2d880",
        "errorCode": "NO_OPENING",
        "requestTime": "2024-10-11T23:24:39Z"
    }
    */

    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; } = default!;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = default!;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = default!;

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? Extras { get; set; } = null;
}
