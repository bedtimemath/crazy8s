using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.DTOs;

public class ApplicationDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => ApplicationId ?? 0;
    public override string Display => String.Join(" ", new [] { ApplicantFirstName, ApplicantLastName, OrganizationName }) 
                                      ?? SharedConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.Status == null) errors.Add("Status is required.");
        if (this.ApplicantType == null) errors.Add("ApplicantType is required.");
        if (String.IsNullOrEmpty(this.ApplicantFirstName)) errors.Add("ApplicantFirstName is required.");
        if (String.IsNullOrEmpty(this.ApplicantLastName)) errors.Add("ApplicantLastName is required.");
        if (String.IsNullOrEmpty(this.ApplicantEmail)) errors.Add("ApplicantEmail is required.");
        if (String.IsNullOrEmpty(this.ApplicantPhone)) errors.Add("ApplicantPhone is required.");
        if (String.IsNullOrEmpty(this.ApplicantTimeZone)) errors.Add("ApplicantTimeZone is required.");
        if (String.IsNullOrEmpty(this.OrganizationName)) errors.Add("OrganizationName is required.");
        if (this.OrganizationType == null) errors.Add("OrganizationType is required.");
        return errors;
    }
    #endregion

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

    #region Child Properties
    public ICollection<ApplicationClubDTO> ApplicationClubs { get; set; } = default!;
    #endregion

    #region Sibling Properties
    public int? LinkedCoachId { get; set; } = default!;
    public CoachDTO? LinkedCoach { get; set; } = default!;
    
    public int? LinkedOrganizationId { get; set; } = default!;
    public OrganizationDTO? LinkedOrganization { get; set; } = default!;
    #endregion
}