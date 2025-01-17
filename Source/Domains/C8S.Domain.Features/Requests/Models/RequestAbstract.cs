using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public record RequestAbstract : RequestBase
{
    public string? PersonFirstName { get; init; }
    public string? OrganizationName { get; init; }
    public string? OrganizationCity { get; init; }
    public string? OrganizationState { get; init; }
    public int? FullSlateAppointmentId { get; init; }
    public DateTimeOffset? FullSlateAppointmentStartsOn { get; init; }
    public List<DateOnly> StartDates { get; init; } = [];
    public DateTimeOffset SubmittedOn { get; init; }

    [JsonIgnore]
    public string PersonFullName =>
        string.Join(" ", new List<string?> { PersonFirstName, PersonLastName }
            .Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string OrganizationCityState =>
        string.Join(", ", new List<string?> { OrganizationCity, OrganizationState }
            .Where(s => !string.IsNullOrEmpty(s)));
}