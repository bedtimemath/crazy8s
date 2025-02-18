using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Orders")]
public class OrderDb: BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OrderId;
    [NotMapped] 
    public override string Display => Number.ToString("00000000");
    #endregion

    #region Id Property
    [Required] 
    public int OrderId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemShippingAddressId { get; set; } = null;

    public Guid? OldSystemClubId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required]
    public int Number { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string? ContactName { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string? ContactEmail { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? ContactPhone { get; set; } = null;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string Recipient { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string Line1 { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? Line2 { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string City { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string State { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string ZIPCode { get; set; } = null!;

    [Required]
    public bool IsMilitary { get; set; } = false!;
    
    [Required]
    public DateTimeOffset OrderedOn { get; set; } = default!;
    
    public DateOnly? ArriveBy { get; set; } = null!;
    
    public DateTimeOffset? ShippedOn { get; set; } = null!;
    
    public DateTimeOffset? EmailedOn { get; set; } = null!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Club))]
    public int? ClubId { get; set; } = null!;
    public ClubDb? Club { get; set; } = null!;
    #endregion

    #region Child Properties
    public ICollection<ShipmentDb> Shipments { get; set; } = null!;
    public ICollection<OrderSkuDb> OrderSkus { get; set; } = null!;
    public ICollection<OrderNoteDb> Notes { get; set; } = null!;
    #endregion
}