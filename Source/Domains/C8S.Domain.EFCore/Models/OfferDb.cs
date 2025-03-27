using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Offers")]
public class OfferDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OfferId;
    [NotMapped] 
    public override string Display => $"[{FulcoId}] {Title}";
    #endregion

    #region Id Property
    [Required] 
    public int OfferId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
    public string FulcoId { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OfferStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Description)]
    public string? Description { get; set; }
    #endregion

    #region Reference Properties
    public KitDb Kit { get; set; } = null!;
    #endregion
}