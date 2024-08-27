using C8S.Applications.Converters;
using System.Text.Json.Serialization;

namespace C8S.Applications.Models;

[Serializable]
public class SubmittedApplication
{
    #region Blob Properties
    public DateTimeOffset? CreatedOn { get; set; }
    #endregion

    #region Form Properties
    [JsonPropertyName("field7")]
    public string FullName { get; set; } = String.Empty;

    [JsonPropertyName("field7_first")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? ApplicantFirstName { get; set; }

    [JsonPropertyName("field7_last")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? ApplicantLastName { get; set; }

    [JsonPropertyName("field9")]
    public string ApplicantEmail { get; set; } = String.Empty;

    [JsonPropertyName("field10")]
    public string ApplicantPhone { get; set; } = String.Empty;

    [JsonPropertyName("field6")]
    public string CoachOrSupervisorString { get; set; } = String.Empty; 

    [JsonPropertyName("field11")]
    public string HostedBeforeString { get; set; } = String.Empty;

    [JsonPropertyName("field19")]
    public string OrganizationName { get; set; } = String.Empty;

    [JsonPropertyName("field20")]
    public string OrganizationAddress { get; set; } = String.Empty;

    [JsonPropertyName("field20_address1")]
    public string AddressLine1 { get; set; } = String.Empty;

    [JsonPropertyName("field20_address2")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("field20_city")]
    public string AddressCity { get; set; } = String.Empty;

    [JsonPropertyName("field20_state")]
    public string AddressState { get; set; } = String.Empty;

    [JsonPropertyName("field20_region")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? AddressRegion { get; set; }

    [JsonPropertyName("field20_postal")]
    public string AddressPostal { get; set; } = String.Empty;

    [JsonPropertyName("field20_country")]
    public string AddressCountry { get; set; } = String.Empty;

    [JsonPropertyName("field21")]
    public string OrganizationType { get; set; } = String.Empty;

    [JsonPropertyName("field22")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? OrganizationTypeOther { get; set; }

    [JsonPropertyName("field24")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? OrganizationTaxIdentifier { get; set; }

    [JsonPropertyName("field48")]
    public string ClubListString { get; set; } = String.Empty;

    [JsonPropertyName("field34")]
    public string HasWorkshopCodeString { get; set; } = String.Empty;

    [JsonPropertyName("field23")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? WorkshopCode { get; set; }

    [JsonPropertyName("field38")]
    public string ReferenceSource { get; set; } = String.Empty;

    [JsonPropertyName("field46")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? ReferenceSourceOther { get; set; }

    [JsonPropertyName("field39")]
    [JsonConverter(typeof(JsonNullableStringConverter))]
    public string? Comments { get; set; }
    #endregion

    #region Derived Properties
    [JsonIgnore]
    public bool HasHostedBefore => HostedBeforeString switch
    {
        CoachAppConstants.HasHostedBefore.FirstTime => false,
        CoachAppConstants.HasHostedBefore.HostedBefore => true,
        _ => throw new ArgumentOutOfRangeException(nameof(HostedBeforeString), $"Unrecognized HostedBefore response: {HostedBeforeString}")
    };

    [JsonIgnore]
    public bool IsSupervisor => CoachOrSupervisorString switch
    { 
        CoachAppConstants.IsSupervisor.Coach => false,
        CoachAppConstants.IsSupervisor.Supervisor => true,
        _ => throw new ArgumentOutOfRangeException(nameof(CoachOrSupervisorString), $"Unrecognized CoachOrSupervisor response: {CoachOrSupervisorString}")
    };

    [JsonIgnore]
    public bool HasWorkshopCode => HasWorkshopCodeString switch
    {
        CoachAppConstants.HasWorkshopCode.No => false,
        CoachAppConstants.HasWorkshopCode.Yes => true,
        _ => throw new ArgumentOutOfRangeException(nameof(HasWorkshopCodeString), $"Unrecognized HasWorkshopCode response: {HasWorkshopCodeString}")
    };
    #endregion 
}