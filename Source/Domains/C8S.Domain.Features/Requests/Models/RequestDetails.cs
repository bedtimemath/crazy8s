using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public class RequestDetails : RequestBase
{
    public string? PersonFirstName { get; set; } = null;
    public DateTimeOffset SubmittedOn { get; set; }
    public string? PlaceName { get; set; } = null;
    public string? PlaceCity { get; set; } = null;
    public string? PlaceState { get; set; } = null;

    [JsonIgnore] 
    public string PersonFullName => 
        String.Join(" ", (new List<string?> {PersonFirstName, PersonLastName})
            .Where(s => !String.IsNullOrEmpty(s)));

    [JsonIgnore] 
    public string OrganizationCityState => 
        String.Join(", ", (new List<string?> {PlaceCity, PlaceState})
            .Where(s => !String.IsNullOrEmpty(s)));
}
