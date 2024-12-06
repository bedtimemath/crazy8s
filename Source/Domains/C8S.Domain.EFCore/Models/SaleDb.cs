using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Sales")]
public class SaleDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => SaleId;
    [NotMapped] 
    public override string Display => SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int SaleId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SaleStatus Status { get; set; } = SaleStatus.Potential;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; } = default!;
    public PlaceDb? Place { get; set; } = default!;
    #endregion

    #region Reference Collections
    public ICollection<ClubDb> Clubs { get; set; } = default!;
    public ICollection<SalePersonDb> SalePersons { get; set; } = default!;
    #endregion
}