using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.DTOs;

public class UnfinishedDTO : BaseDTO
{
    #region Property Overrides
    public override int Id => UnfinishedId ?? 0;
    public override string Display => String.Join(" ", new[] { ApplicantFirstName, ApplicantLastName, OrganizationName })
                                      ?? SharedConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.Code == null) errors.Add("Code is required.");
        if (String.IsNullOrEmpty(this.ApplicantLastName)) errors.Add("ApplicantLastName is required.");
        if (String.IsNullOrEmpty(this.ApplicantEmail)) errors.Add("ApplicantEmail is required.");
        if (String.IsNullOrEmpty(this.ApplicantTimeZone)) errors.Add("ApplicantTimeZone is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? UnfinishedId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? Code { get; set; } = null;

    public ApplicantType? ApplicantType { get; set; } = null;

    public string? ApplicantFirstName { get; set; } = null;

    public string? ApplicantLastName { get; set; } = null;

    public string? ApplicantEmail { get; set; } = null;

    public string? ApplicantPhone { get; set; } = null;

    public string? ApplicantTimeZone { get; set; } = null;

    public bool? HasHostedBefore { get; set; } = null;

    public string? OrganizationName { get; set; } = null;

    public string? OrganizationAddress1 { get; set; } = null;

    public string? OrganizationAddress2 { get; set; } = null;

    public string? OrganizationCity { get; set; } = null;

    public string? OrganizationState { get; set; } = null;

    public string? OrganizationPostalCode { get; set; } = null;

    public OrganizationType? OrganizationType { get; set; } = null;

    public string? OrganizationTypeOther { get; set; } = null;

    public string? OrganizationTaxIdentifier { get; set; } = null;

    public string? ClubsString { get; set; } = null;

    public string? WorkshopCode { get; set; } = null;

    public DateTimeOffset? ChosenTimeSlot { get; set; } = null;

    public string? ReferenceSource { get; set; } = null;

    public string? ReferenceSourceOther { get; set; } = null;

    public string? Comments { get; set; } = null;

    public DateTimeOffset? EndPart01On { get; set; } = null;

    public DateTimeOffset? EndPart02On { get; set; } = null;

    public DateTimeOffset? EndPart03On { get; set; } = null;

    public DateTimeOffset? EndPart04On { get; set; } = null;

    public DateTimeOffset? EndPart05On { get; set; } = null;

    public DateTimeOffset? SubmittedOn { get; set; } = null;
    #endregion

    #region Derived Properties
    public string? ApplicantFullName => 
        String.IsNullOrEmpty(ApplicantFirstName) && String.IsNullOrEmpty(ApplicantLastName) ? null : 
            String.Join(" ", new List<string?>() { ApplicantFirstName, ApplicantLastName });
    #endregion
}