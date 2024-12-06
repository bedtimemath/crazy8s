using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Places")]
public class PlaceDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => PlaceId;
    [NotMapped] 
    public override string Display => Name;
    #endregion

    #region Id Property
    [Required] 
    public int PlaceId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemCompanyId { get; set; } = null;
    
    public Guid? OldSystemOrganizationId { get; set; } = null;
    
    public Guid? OldSystemPostalAddressId { get; set; } = null;
    
    public Guid? OldSystemUsaPostalId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType Type { get; set; } = PlaceType.Other;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? TypeOther { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? TaxIdentifier { get; set; } = null;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string Line1 { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? Line2 { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string City { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string State { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string ZIPCode { get; set; } = default!;

    [Required]
    public bool IsMilitary { get; set; } = default!;
    #endregion

    #region Child Properties
    // one-to-many
    public ICollection<ClubDb> Clubs { get; set; } = default!;
    public ICollection<PersonDb> Persons { get; set; } = default!;
    public ICollection<RequestDb> Requests { get; set; } = default!;
    public ICollection<SaleDb> Sales { get; set; } = default!;
    #endregion
}