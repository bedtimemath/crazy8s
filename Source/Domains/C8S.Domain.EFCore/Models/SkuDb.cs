using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;
using SC.Common.Extensions;

namespace C8S.Domain.EFCore.Models;

[Table("Skus")]
public class SkuDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => SkuId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { Season.ToString(), AgeLevel.GetLabel(), ClubSize.GetLabel() }) 
                                       ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int SkuId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemSkuId { get; set; } = null;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
    public string Key { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; set; } = default!;

    [Required]
    public int Season { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize ClubSize { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Child Properties
    public ICollection<OrderSkuDb> OrderSkus { get; set; } = default!;
    #endregion
}