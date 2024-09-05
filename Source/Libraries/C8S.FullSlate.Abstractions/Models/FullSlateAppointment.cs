using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateAppointment
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("occurrence_key")]
    public string OccurrenceKey { get; set; } = default!;

    [JsonPropertyName("employee")]
    public FullSlateEmployee Employee { get; set; } = default!;

    [JsonPropertyName("client")]
    public FullSlateClient Client { get; set; } = default!;

    [JsonPropertyName("services")]
    public List<FullSlateOffering> Offerings { get; set; } = new();


}