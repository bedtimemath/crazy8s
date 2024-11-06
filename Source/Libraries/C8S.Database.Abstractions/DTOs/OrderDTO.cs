using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;
using SC.Common;

namespace C8S.Database.Abstractions.DTOs;

public class OrderDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => OrderId ?? 0;
    public override string Display => Number?.ToString("00000000") ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.Number == null) errors.Add("Number is required.");
        if (this.Status == null) errors.Add("Status is required.");
        if (this.OrderedOn == null) errors.Add("OrderedOn is required.");
        if (this.ArriveBy == null) errors.Add("ArriveBy is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? OrderId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemShippingAddressId { get; set; } = null;

    public Guid? OldSystemClubId { get; set; } = null;

    public int? Number { get; set; } = default!;

    public OrderStatus? Status { get; set; } = OrderStatus.Ordered;

    public string? ContactEmail { get; set; } = null;

    public string? ContactPhone { get; set; } = null;

    public string? ContactPhoneExt { get; set; } = null;
    
    public DateTimeOffset? OrderedOn { get; set; } = default!;
    
    public DateOnly? ArriveBy { get; set; } = default!;
    
    public DateTimeOffset? ShippedOn { get; set; } = default!;
    
    public DateTimeOffset? EmailedOn { get; set; } = default!;
    
    public Guid? BatchIdentifier { get; set; } = default!;

    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    public int? AddressId { get; set; } = null;
    public AddressDTO? Address { get; set; } = null;

    public int? ClubId { get; set; } = null;
    public ClubDTO? Club { get; set; } = null;
    #endregion

    #region Child Properties
    public ICollection<SkuDTO> Skus { get; set; } = default!;
    #endregion
}