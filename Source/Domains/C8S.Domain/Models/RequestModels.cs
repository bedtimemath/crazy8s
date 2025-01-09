using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Domain.Models;

public class RequestBase
{
    #region Id Property
    public int RequestId { get; set; }
    public RequestStatus Status { get; set; }
    public string ApplicantLastName { get; set; } = default!;
    public string ApplicantEmail { get; set; } = default!;
    #endregion
}

public class RequestListDisplay : RequestBase
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