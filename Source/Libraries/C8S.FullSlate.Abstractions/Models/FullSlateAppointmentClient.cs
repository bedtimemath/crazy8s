using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateAppointmentClient
{
    //id	number
    //example: 8
    //Client ID
    [JsonPropertyName("id")]
    public int Id { get; set; }

    //name	string
    //example: Johnny Walker
    //Client's name
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    //phone_number	string
    //example: 1235551234
    //Client's phone number.
    [JsonPropertyName("phone_number")]
    public string? Phone { get; set; } = null;

    //email	string
    //example: mr.client@example.com
    //Client's email.
    [JsonPropertyName("email")]
    public string? Email { get; set; } = null;
}