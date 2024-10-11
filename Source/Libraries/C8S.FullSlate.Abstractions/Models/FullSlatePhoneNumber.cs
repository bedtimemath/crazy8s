using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlatePhoneNumber
{
    //number*	string
    //example: 1255464478
    //Phone number
    [JsonPropertyName("number")]
    public string Number { get; set; } = default!;

    //contact_type	string
    //Enum: Array [ 9 ]
    //example: WORK
    //Phone contact type. Options: HOME | MOBILE | WORK | FAX | PAGER | OTHER | PRIMARY | SECONDARY
    [JsonPropertyName("contact_type")]
    public string? ContactTypeString { get; set; } = null;
}