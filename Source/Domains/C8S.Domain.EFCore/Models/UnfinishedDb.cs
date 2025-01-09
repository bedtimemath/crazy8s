using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Unfinisheds")]
public class UnfinishedDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped]
    public override int Id => UnfinishedId;
    [NotMapped]
    public override string Display => String.Join(" ", [PersonFirstName, PersonLastName, PlaceName])
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
    public ApplicantType? PersonType { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonFirstName { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonLastName { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string? PersonEmail { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? PersonPhone { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PersonTimeZone { get; set; }

    public bool? HasHostedBefore { get; set; }

    public bool? AddressHasChanged { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string? PlaceName { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? PlaceAddress1 { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? PlaceAddress2 { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceCity { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string? PlaceState { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string? PlacePostalCode { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType? PlaceType { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceTypeOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PlaceTaxIdentifier { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.XLong)]
    public string? ClubsString { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; }

    public DateTimeOffset? ChosenTimeSlot { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; }

    public DateTimeOffset? EndPart01On { get; set; }

    public DateTimeOffset? EndPart02On { get; set; }

    public DateTimeOffset? EndPart03On { get; set; }

    public DateTimeOffset? EndPart04On { get; set; }

    public DateTimeOffset? SubmittedOn { get; set; }

    public int? PersonId { get; set; }

    public int? PlaceId { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Request))]
    public int? RequestId { get; set; }
    public RequestDb? Request { get; set; }
    #endregion
}