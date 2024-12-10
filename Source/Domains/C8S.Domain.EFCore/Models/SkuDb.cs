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
    public override string Display => String.Join(" ", new [] { Season?.ToString(), AgeLevel?.GetLabel(), ClubSize?.GetLabel() }
                                           .Select(s => !String.IsNullOrEmpty(s)));
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
    public string Key { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; set; } = default!;

    public int? Season { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel? AgeLevel { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize? ClubSize { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.XLong)]
    public string? Comments { get; set; }
    #endregion

    #region Child Properties
    public ICollection<OrderSkuDb> OrderSkus { get; set; } = default!;
    public ICollection<PermissionDb> Permissions { get; set; } = default!;
    #endregion
}