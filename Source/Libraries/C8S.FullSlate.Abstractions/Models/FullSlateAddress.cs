using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlateAddress
{
    //street1*	string
    //example: SUITE 5A-1204
    //Street 1
    [JsonPropertyName("street1")]
    public string Street1 { get; set; } = default!;

    //street2	string
    //example: Mountain street
    //Street 2
    [JsonPropertyName("street2")]
    public string? Street2 { get; set; } = null;

    //city	string
    //example: Tucson
    //City
    [JsonPropertyName("city")]
    public string? City { get; set; } = null;

    //state	string
    //example: AZ
    //State
    [JsonPropertyName("state")]
    public string? State { get; set; } = null;

    //postal_code	string
    //example: 85705
    //Postal Code
    [JsonPropertyName("postal_code")]
    public string? PostalCode { get; set; } = null;
}