using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    #region Database Properties (Old System)
    public Guid? OldSystemOrderSkuId { get; set; } = null;
    
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemSkuId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required]
    public int Ordinal { get; set; }

    [Required]
    public short Quantity { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    public OrderDb Order { get; set; } = null!;

    [ForeignKey(nameof(Sku))]
    public int SkuId { get; set; }
    public SkuDb Sku { get; set; } = null!;
    #endregion
}