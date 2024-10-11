using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateClientCc
{
    //id	string
    //example: 15
    //The client's parent ID
    [JsonPropertyName("id")]
    public string IdString { get; set; }

    //first_name	string
    //example: Thomas
    //The client's parent first name
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = default!;

    //last_name	string
    //example: Edison
    //The client's parent last name
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = default!;

    //email	string
    //example: parent1@example.com
    //The client's parent email address
    [JsonPropertyName("email")]
    public string? Email { get; set; } = null;
}



