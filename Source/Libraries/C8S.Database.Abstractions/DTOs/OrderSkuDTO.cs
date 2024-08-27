using C8S.Common;
using C8S.Database.Abstractions.Base;

namespace C8S.Database.Abstractions.DTOs;

public class OrderSkuDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => OrderSkuId ?? 0;
    public override string Display => Quantity?.ToString() ?? SharedConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.Ordinal == null) errors.Add("Ordinal is required.");
        if (this.Quantity == null) errors.Add("Quantity is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? OrderSkuId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderSkuId { get; set; } = null;
    
    public Guid? OldSystemOrderId { get; set; } = null;

    public Guid? OldSystemSkuId { get; set; } = null;

    public int? Ordinal { get; set; } = default!;

    public short? Quantity { get; set; } = default!;
    #endregion

    #region Reference Properties
    public int? OrderId { get; set; } = null;
    public OrderDTO? Order { get; set; } = null;

    public int? SkuId { get; set; } = null;
    public SkuDTO? Sku { get; set; } = null;
    #endregion
}