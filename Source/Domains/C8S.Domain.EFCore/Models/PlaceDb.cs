using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Places")]
public class PlaceDb: BaseCoreDb
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

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaceType Type { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? TypeOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? TaxIdentifier { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string Line1 { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? Line2 { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string City { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string State { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string ZIPCode { get; set; } = null!;

    [Required]
    public bool IsMilitary { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Parent))]
    public int? ParentId { get; set; }
    public PlaceDb? Parent { get; set; }
    #endregion
    
    #region Reference Collections
    public ICollection<PlaceDb> Children { get; set; } = null!;

    public ICollection<ClubDb> Clubs { get; set; } = null!;
    public ICollection<PersonDb> Persons { get; set; } = null!;
    public ICollection<TicketDb> Tickets { get; set; } = null!;
    public ICollection<PlaceNoteDb> Notes { get; set; } = null!;
    #endregion
}