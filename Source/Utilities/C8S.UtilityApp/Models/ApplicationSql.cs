using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;
using C8S.Domain.Features.Requests.Enums;
using AppType = C8S.Domain.Enums.ApplicantType;

namespace C8S.UtilityApp.Models;

public class ApplicationSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT a.[Id] AS [OldSystemApplicationId], a.[NewOrganizationAddressId] AS [OldSystemAddressId], a.[LinkedCoachId] AS [OldSystemLinkedCoachId], a.[LinkedOrganizationId] AS [OldSystemLinkedOrganizationId], aps.[Name] AS [StatusString], ct.[Name] AS [ApplicantTypeString], a.[CoachFirstName] AS [ApplicantFirstName], a.[CoachLastName] AS [ApplicantLastName], a.[CoachEmail] AS [ApplicantEmail], a.[CoachPhone] AS [ApplicantPhoneString], a.[CoachPhoneExt] AS [ApplicantPhoneExt], a.[CoachTimeZoneId] AS [ApplicantTimeZone], a.[NewOrganizationName] AS [OrganizationName], ot.[Name] AS [OrganizationTypeString], a.[NewOrganizationOrganizationTypeOther] AS [OrganizationTypeOther], a.[NewOrganizationTaxId] AS [OrganizationTaxIdentifier], a.[WorkshopCode], a.[AppointmentId], a.[Comments], a.[Submitted] AS [SubmittedOn], a.[Notes], CAST(a.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Application] a  LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId] LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId] LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId] WHERE a.[DeletedBy] IS NULL AND a.[ApplicationStatusId] IS NOT NULL";
    #endregion

    #region Id Property
    public int? ApplicationId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemApplicationId { get; set; } = null;

    public Guid? OldSystemAddressId { get; set; } = null;

    public Guid? OldSystemLinkedCoachId { get; set; } = null;

    public Guid? OldSystemLinkedOrganizationId { get; set; } = null;

    [NotMapped]
    public string? StatusString { get; set; } = null;

    [NotMapped]
    public string? ApplicantTypeString { get; set; } = null;

    public string? ApplicantFirstName { get; set; } = null;

    public string? ApplicantLastName { get; set; } = null;

    public string? ApplicantEmail { get; set; } = null;

    [NotMapped]
    public string? ApplicantPhoneString { get; set; } = null;

    public string? ApplicantPhoneExt { get; set; } = null;

    public string? ApplicantTimeZone { get; set; } = null;

    public string? OrganizationName { get; set; } = null;

    [NotMapped]
    public string? OrganizationTypeString { get; set; } = null;

    public string? OrganizationTypeOther { get; set; } = null;

    public string? OrganizationTaxIdentifier { get; set; } = null;

    public string? WorkshopCode { get; set; } = null;

    public long? AppointmentId { get; set; } = null;

    public string? Comments { get; set; } = null;

    public DateTimeOffset? SubmittedOn { get; set; } = null;

    public string? Notes { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public string? ApplicantPhone => (ApplicantPhoneString?.Length ?? 0) < 10 ? ApplicantPhoneString :
        ApplicantPhoneString switch
        {
            "0000000000" or null => null,
            _ => $"({ApplicantPhoneString.Substring(0,3)}) {ApplicantPhoneString.Substring(3,3)}-{ApplicantPhoneString.Substring(6,4)}"
        };

    public RequestStatus? Status => StatusString switch
    {
        "Approved" => RequestStatus.Approved,
        "Deleted" => RequestStatus.Deleted,
        "Denied" => RequestStatus.Denied,
        "Future" => RequestStatus.Future,
        "Pending" => RequestStatus.Pending,
        "Received" => RequestStatus.Received,
        null => null,
        _ => throw new Exception($"Unrecognized: {StatusString}")
    };

    public ApplicantType? ApplicantType => ApplicantTypeString switch
    {
        "Coach" => AppType.Coach,
        "Mentor" => AppType.Mentor,
        "Student" => AppType.Student,
        "Supervisor" => AppType.Supervisor,
        null => null,
        _ => throw new Exception($"Unrecognized: {ApplicantTypeString}")
    };

    public PlaceType? OrganizationType => OrganizationTypeString switch
    {
        "Boys & Girls Club" => PlaceType.BoysGirlsClub,
        "Home School Co-Op" => PlaceType.HomeSchool,
        "Library" => PlaceType.Library,
        "Other" => PlaceType.Other,
        "School" => PlaceType.School,
        "YMCA" => PlaceType.YMCA,
        _ => throw new Exception($"Unrecognized: {OrganizationTypeString}")
    };

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}