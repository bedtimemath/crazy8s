using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateEmployee
{
    //id	number
    //example: 11
    //The Employee ID used to reference the employee in other API calls
    [JsonPropertyName("id")]
    public int Id { get; set; }

    //name    string
    //example: Sandy Wayne
    //Employee name
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    //location_id number
    //example: 3
    //Location ID
    [JsonPropertyName("location_id")]
    public int LocationId { get; set; }
}