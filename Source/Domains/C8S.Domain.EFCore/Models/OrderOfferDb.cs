using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("OrderOffers")]
public class OrderOfferDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OrderOfferId;
    [NotMapped] 
    public override string Display => $"{Order}<=>{Offer} [{Quantity}]";
    #endregion

    #region Id Property
    [Required] 
    public int OrderOfferId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public int Ordinal { get; set; }

    [Required]
    public int Quantity { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    public OrderDb Order { get; set; } = null!;

    [ForeignKey(nameof(Offer))]
    public int OfferId { get; set; }
    public OfferDb Offer { get; set; } = null!;
    #endregion
}