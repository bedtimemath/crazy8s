using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Unfinisheds")]
public class UnfinishedDb : BaseDb
{
    #region Override Properties
    [NotMapped]
    public override int Id => UnfinishedId;
    [NotMapped]
    public override string Display => String.Join(" ", new[] { ApplicantFirstName, ApplicantLastName, OrganizationName })
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required]
    public int UnfinishedId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public Guid Code { get; set; } = Guid.NewGuid();

    [MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicantType? ApplicantType { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Name)]
    public string? ApplicantFirstName { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Name)]
    public string? ApplicantLastName { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Email)]
    public string? ApplicantEmail { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? ApplicantPhone { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? ApplicantTimeZone { get; set; } = default!;

    public bool? HasHostedBefore { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.FullName)]
    public string? OrganizationName { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Standard)]
    public string? OrganizationAddress1 { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Standard)]
    public string? OrganizationAddress2 { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? OrganizationCity { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Tiny)]
    public string? OrganizationState { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.ZIPCode)]
    public string? OrganizationPostalCode { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationType? OrganizationType { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? OrganizationTypeOther { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? OrganizationTaxIdentifier { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.XLong)]
    public string? ClubsString { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; } = null;

    public DateTimeOffset? ChosenTimeSlot { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; } = null;

    public DateTimeOffset? EndPart01On { get; set; }

    public DateTimeOffset? EndPart02On { get; set; }

    public DateTimeOffset? EndPart03On { get; set; }

    public DateTimeOffset? EndPart04On { get; set; }

    public DateTimeOffset? EndPart05On { get; set; }

    public DateTimeOffset? SubmittedOn { get; set; }
    #endregion
}