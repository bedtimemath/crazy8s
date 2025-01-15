using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public class RequestListItem : RequestBase
{
    public string? PersonFirstName { get; set; } = null;
    public DateTimeOffset SubmittedOn { get; set; }
    public string? OrganizationName { get; set; } = null;
    public string? OrganizationCity { get; set; } = null;
    public string? OrganizationState { get; set; } = null;

    [JsonIgnore] 
    public string PersonFullName => 
        String.Join(" ", (new List<string?> {PersonFirstName, PersonLastName})
            .Where(s => !String.IsNullOrEmpty(s)));

    [JsonIgnore] 
    public string OrganizationCityState => 
        String.Join(", ", (new List<string?> {OrganizationCity, OrganizationState})
            .Where(s => !String.IsNullOrEmpty(s)));
}
