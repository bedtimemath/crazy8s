using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Common.Extensions;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Skus")]
public class SkuDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => SkuId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { Season.ToString(), AgeLevel.GetLabel(), ClubSize.GetLabel() }) 
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int SkuId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemSkuId { get; set; } = null;

    [Required, MaxLength(SharedConstants.MaxLengths.Key)]
    public string Key { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Name)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkuStatus Status { get; set; } = default!;

    [Required]
    public int Season { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize ClubSize { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Parent Properties
    public ICollection<OrderDb> Orders { get; set; } = default!;
    #endregion
}