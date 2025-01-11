using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Lists;

public class RequestListItem : RequestBase
{
    public string? ApplicantFirstName { get; set; } = null;
    public DateTimeOffset SubmittedOn { get; set; }
    public string? OrganizationName { get; set; } = null;
    public string? OrganizationCity { get; set; } = null;
    public string? OrganizationState { get; set; } = null;

    [JsonIgnore] 
    public string ApplicantFullName => 
        String.Join(" ", (new List<string?> {ApplicantFirstName, ApplicantLastName})
            .Where(s => !String.IsNullOrEmpty(s)));

    [JsonIgnore] 
    public string OrganizationCityState => 
        String.Join(", ", (new List<string?> {OrganizationCity, OrganizationState})
            .Where(s => !String.IsNullOrEmpty(s)));
}
