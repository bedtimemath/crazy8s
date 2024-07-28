using C8S.Database.Abstractions.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class ApplicationSql
{
    #region Id Property
    public int? ApplicationId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemApplicationId { get; set; } = null;

    public ApplicationStatus? Status { get; set; } = null;

    public ApplicantType? ApplicantType { get; set; } = null;

    public string? ApplicantFirstName { get; set; } = null;

    public string? ApplicantLastName { get; set; } = null;

    public string? ApplicantEmail { get; set; } = null;

    public string? ApplicantPhone { get; set; } = null;

    public string? ApplicantPhoneExt { get; set; } = null;

    public string? ApplicantTimeZone { get; set; } = null;

    public string? OrganizationName { get; set; } = null;

    public OrganizationType? OrganizationType { get; set; } = null;

    public string? OrganizationTypeOther { get; set; } = null;

    public string? OrganizationTaxIdentifier { get; set; } = null;

    public string? WorkshopCode { get; set; } = null;

    public string? Comments { get; set; } = null;

    public DateTimeOffset? SubmittedOn { get; set; } = null;

    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Derived Properties
#if false
    public ApplicationType Type =>
    OldSystemType switch
    {
        "Boys & Girls Club" => ApplicationType.BoysGirlsClub,
        "Home School Co-Op" => ApplicationType.HomeSchool,
        "Library" => ApplicationType.Library,
        "Other" => ApplicationType.Other,
        "School" => ApplicationType.School,
        "YMCA" => ApplicationType.YMCA,
        _ => throw new Exception("Unrecognized")
    }; 
#endif

    #endregion
}