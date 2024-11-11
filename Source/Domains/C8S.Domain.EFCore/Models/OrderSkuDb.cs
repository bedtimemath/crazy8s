using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Base;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("OrderSkus")]
public class OrderSkuDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OrderSkuId;
    [NotMapped] 
    public override string Display => Quantity.ToString();
    #endregion

    #region Id Property
    [Required] 
    public int OrderSkuId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemOrderSkuId { get; set; } = null;
    
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemSkuId { get; set; } = null;

    [Required]
    public int Ordinal { get; set; } = default!;

    [Required]
    public short Quantity { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; } = default!;
    public OrderDb Order { get; set; } = default!;

    [ForeignKey(nameof(Sku))]
    public int SkuId { get; set; } = default!;
    public SkuDb Sku { get; set; } = default!;
    #endregion
}