using C8S.Domain.Features.Requests.Enums;
using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public record RequestListItem(
    // from base record
    int RequestId,
    RequestStatus Status,
    string PersonLastName,
    string PersonEmail,
    // added for this record
    string? PersonFirstName,
    DateTimeOffset SubmittedOn,
    string? OrganizationName,
    string? OrganizationCity,
    string? OrganizationState
) : RequestBase(RequestId, Status, PersonLastName, PersonEmail)
{
    [JsonIgnore] 
    public string PersonFullName => 
        String.Join(" ", (new List<string?> {PersonFirstName, PersonLastName})
            .Where(s => !String.IsNullOrEmpty(s)));

    [JsonIgnore] 
    public string OrganizationCityState => 
        String.Join(", ", (new List<string?> {OrganizationCity, OrganizationState})
            .Where(s => !String.IsNullOrEmpty(s)));
}
