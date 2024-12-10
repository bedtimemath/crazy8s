using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Requests")]
public class RequestDb : BaseCoreDb
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
    public Guid? OldSystemApplicationId { get; set; }
    
    public Guid? OldSystemAddressId { get; set; }
    
    public Guid? OldSystemLinkedCoachId { get; set; }
    
    public Guid? OldSystemLinkedOrganizationId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RequestStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicantType? PersonType { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? PersonFirstName { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string PersonLastName { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string PersonEmail { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PersonPhone { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string PersonTimeZone { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string? PlaceName { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType? PlaceType { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? PlaceTypeOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? PlaceTaxIdentifier { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Comments { get; set; }

    [Required]
    public DateTimeOffset SubmittedOn { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int? PersonId { get; set; }
    public PersonDb? Person { get; set; }

    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; }
    public PlaceDb? Place { get; set; }

    [ForeignKey(nameof(Sale))]
    public int? SaleId { get; set; }
    public SaleDb? Sale { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ProposedClubDb> ProposedClubs { get; set; } = default!;
    public ICollection<RequestNoteDb> Notes { get; set; } = default!;
    #endregion
}