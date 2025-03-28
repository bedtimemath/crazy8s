using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using C8S.Domain.Interfaces;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Kits")]
public class KitDb : BaseCoreDb, IKit
{
    #region Override Properties
    [NotMapped] 
    public override int Id => KitId;
    [NotMapped] 
    public override string Display => Key;
    #endregion

    #region Id Property
    [Required] 
    public int KitId { get; set; }
    #endregion

    #region Database Properties
    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string Key { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KitStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string Year { get; set; } = null!;

    public int Season { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Shorter)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string? Version { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Comments)]
    public string? Comments { get; set; }
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Offer))]
    public int OfferId { get; set; }
    public OfferDb Offer { get; set; } = null!;
    
    [Required, ForeignKey(nameof(KitPage))]
    public int? KitPageId { get; set; }
    public KitPageDb? KitPage { get; set; } = null!;
    #endregion

    #region Child Properties
    public ICollection<ClubDb> Clubs { get; set; } = null!;
    #endregion
}