using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Requests")]
public class RequestDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ApplicationId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { PersonFirstName, PersonLastName, PlaceName }) 
                                       ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int ApplicationId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemApplicationId { get; set; } = null;
    
    public Guid? OldSystemAddressId { get; set; } = null;
    
    public Guid? OldSystemLinkedCoachId { get; set; } = null;
    
    public Guid? OldSystemLinkedOrganizationId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RequestStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicantType? PersonType { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonFirstName { get; set; } = null;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string PersonLastName { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string PersonEmail { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PersonPhone { get; set; } = null;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string PersonTimeZone { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string? PlaceName { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType? PlaceType { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceTypeOther { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PlaceTaxIdentifier { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; } = null;

    [Required]
    public DateTimeOffset SubmittedOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int? PersonId { get; set; } = default!;
    public PersonDb? Person { get; set; } = default!;

    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; } = default!;
    public PlaceDb? Place { get; set; } = default!;
    #endregion

    #region Reference Collections
    // one-to-many
    public ICollection<ProposedClubDb> ProposedClubs { get; set; } = default!;
    #endregion
}