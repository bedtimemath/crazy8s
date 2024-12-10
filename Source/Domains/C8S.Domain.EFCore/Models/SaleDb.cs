using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Sales")]
public class SaleDb : BaseCoreDb
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
    public SaleStatus Status { get; set; } = default!;
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Place))]
    public int PlaceId { get; set; } = default!;
    public PlaceDb Place { get; set; } = default!;

    [ForeignKey(nameof(Request))]
    public int? RequestId { get; set; }
    public RequestDb? Request { get; set; }

    [ForeignKey(nameof(Invoice))]
    public int? InvoiceId { get; set; }
    public InvoiceDb? Invoice { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ClubDb> Clubs { get; set; } = default!;
    public ICollection<SalePersonDb> SalePersons { get; set; } = default!;
    public ICollection<SaleNoteDb> Notes { get; set; } = default!;
    #endregion
}