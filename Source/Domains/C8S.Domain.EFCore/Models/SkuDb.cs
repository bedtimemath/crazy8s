using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;
using SC.Common.Extensions;

namespace C8S.Domain.EFCore.Models;

[Table("Skus")]
public class SkuDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => SkuId;
    [NotMapped] 
    public override string Display => ClubKey;
    #endregion

    #region Id Property
    [Required] 
    public int SkuId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemSkuId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
    public string FulcoId { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string Year { get; set; } = null!;

    public int Season { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? Version { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.XLong)]
    public string? Comments { get; set; }
    #endregion

    #region Child Properties
    public ICollection<OrderSkuDb> OrderSkus { get; set; } = null!;
    public ICollection<PermissionDb> Permissions { get; set; } = null!;
    #endregion

    #region Derived Properties
    [NotMapped] 
    public string ClubKey => String.Join('-', [Year, Season, AgeLevel, Version]);
    #endregion
}