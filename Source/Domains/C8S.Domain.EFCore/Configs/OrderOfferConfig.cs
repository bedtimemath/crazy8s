using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class OrderOfferConfig : IEntityTypeConfiguration<OrderOfferDb>
{
    public void Configure(EntityTypeBuilder<OrderOfferDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrderOfferId { get; set; }
        entity.HasKey(m => m.OrderOfferId);
        #endregion

        #region Database Properties
        //[Required]
        //public int Ordinal { get; set; }
        entity.Property(m => m.Ordinal)
            .IsRequired(true);

        //[Required]
        //public int Quantity { get; set; }
        entity.Property(m => m.Quantity)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Order))]
        //public int OrderId { get; set; } = null;
        entity.Property(m => m.OrderId)
            .IsRequired(true);
        
        //[ForeignKey(nameof(Offer))]
        //public int OfferId { get; set; } = null;
        entity.Property(m => m.OfferId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public OrderDb Order { get; set; } = null;
        entity.HasOne(m => m.Order)
            .WithMany(m => m.OrderOffers)
            .HasForeignKey(m => m.OrderId)
            .IsRequired(true);

        //public OfferDb Offer { get; set; } = null;
        entity.HasOne(m => m.Offer)
            .WithMany(m => m.OrderOffers)
            .HasForeignKey(m => m.OfferId)
            .IsRequired(true);
        #endregion
    }
}