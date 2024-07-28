using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Applications")]
public class ApplicationDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ApplicationId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { ApplicantFirstName, ApplicantLastName, OrganizationName }) 
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int ApplicationId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemApplicationId { get; set; } = null;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicationStatus Status { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicantType ApplicantType { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Name)]
    public string ApplicantFirstName { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Name)]
    public string ApplicantLastName { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Email)]
    public string ApplicantEmail { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    public string ApplicantPhone { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    public string ApplicantPhoneExt { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Medium)]
    public string ApplicantTimeZone { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.FullName)]
    public string? OrganizationName { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationType? OrganizationType { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? OrganizationTypeOther { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? OrganizationTaxIdentifier { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; } = null;

    [Required]
    public DateTimeOffset SubmittedOn { get; set; }

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(LinkedCoach))]
    public int? LinkedCoachId { get; set; } = default!;
    public CoachDb? LinkedCoach { get; set; } = default!;

    [ForeignKey(nameof(LinkedOrganization))]
    public int? LinkedOrganizationId { get; set; } = default!;
    public OrganizationDb? LinkedOrganization { get; set; } = default!;
    #endregion

    #region Reference Collections
    // one-to-many
    public ICollection<ApplicationClubDb> ApplicationClubs { get; set; } = default!;
    #endregion
}