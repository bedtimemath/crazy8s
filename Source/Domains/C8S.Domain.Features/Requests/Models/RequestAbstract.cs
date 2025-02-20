using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public record RequestAbstract : RequestBase
{
    public string? PersonFirstName { get; init; }

    public string? PlaceName { get; init; }
    public string? PlaceCity { get; init; }
    public string? PlaceState { get; init; }

    public long? FullSlateAppointmentId { get; init; }

    public DateTimeOffset? FullSlateAppointmentStartsOn { get; init; }

    public List<DateOnly> StartDates { get; init; } = [];
    public DateTimeOffset SubmittedOn { get; init; }

    [JsonIgnore]
    public string PersonFullName =>
        string.Join(" ", new List<string?> { PersonFirstName, PersonLastName }
            .Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string PlaceCityState =>
        string.Join(", ", new List<string?> { PlaceCity, PlaceState }
            .Where(s => !string.IsNullOrEmpty(s)));
}