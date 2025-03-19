using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Shipments")]
public class ShipmentDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ShipmentId;
    [NotMapped] 
    public override string Display => TrackingNumber;
    #endregion

    #region Id Property
    [Required] 
    public int ShipmentId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
    public string TrackingNumber { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ShipMethod? ShipMethod { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ShipMethodOther { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; } = null!;
    public OrderDb? Order { get; set; } = null!;
    #endregion
}