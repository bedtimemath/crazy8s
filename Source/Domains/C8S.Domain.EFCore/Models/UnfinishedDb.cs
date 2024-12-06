using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Unfinisheds")]
public class UnfinishedDb : BaseDb
{
    #region Override Properties
    [NotMapped]
    public override int Id => UnfinishedId;
    [NotMapped]
    public override string Display => String.Join(" ", new[] { PersonFirstName, PersonLastName, PlaceName })
                                       ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required]
    public int UnfinishedId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public Guid Code { get; set; } = Guid.NewGuid();

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicantType? PersonType { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonFirstName { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonLastName { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string? PersonEmail { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PersonPhone { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PersonTimeZone { get; set; } = default!;

    public bool? HasHostedBefore { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string? PlaceName { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? PlaceAddress1 { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? PlaceAddress2 { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceCity { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string? PlaceState { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string? PlacePostalCode { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType? PlaceType { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceTypeOther { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PlaceTaxIdentifier { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.XLong)]
    public string? ClubsString { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; } = null;

    public DateTimeOffset? ChosenTimeSlot { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; } = null;

    public DateTimeOffset? EndPart01On { get; set; }

    public DateTimeOffset? EndPart02On { get; set; }

    public DateTimeOffset? EndPart03On { get; set; }

    public DateTimeOffset? EndPart04On { get; set; }

    public DateTimeOffset? SubmittedOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Request))]
    public int? RequestId { get; set; } = default!;
    public RequestDb? Request { get; set; } = default!;
    #endregion
}