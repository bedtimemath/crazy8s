using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Orders")]
public class OrderDb: BaseDb
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

    #region Database Properties
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemShippingAddressId { get; set; } = null;

    public Guid? OldSystemClubId { get; set; } = null;

    [Required]
    public int Number { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;

    [MaxLength(SharedConstants.MaxLengths.Email)]
    public string? ContactEmail { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? ContactPhone { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? ContactPhoneExt { get; set; } = null;
    
    [Required]
    public DateTimeOffset OrderedOn { get; set; } = default!;
    
    [Required]
    public DateOnly ArriveBy { get; set; } = default!;
    
    public DateTimeOffset? ShippedOn { get; set; } = default!;
    
    public DateTimeOffset? EmailedOn { get; set; } = default!;
    
    public Guid? BatchIdentifier { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; } = default!;
    public AddressDb Address { get; set; } = default!;

    [ForeignKey(nameof(Club))]
    public int? ClubId { get; set; } = default!;
    public ClubDb? Club { get; set; } = default!;
    #endregion

    #region Child Properties
    public ICollection<SkuDb> Skus { get; set; } = default!;
    #endregion
}