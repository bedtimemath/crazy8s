using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class OrderSkuConfig : BaseConfig<OrderSkuDb>
{
    public override void Configure(EntityTypeBuilder<OrderSkuDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrderSkuId { get; set; }
        entity.HasKey(m => m.OrderSkuId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemOrderSkuId { get; set; } = null;
        entity.Property(m => m.OldSystemOrderSkuId)
            .IsRequired(false);

        //public Guid? OldSystemOrderId { get; set; } = null;
        entity.Property(m => m.OldSystemOrderId)
            .IsRequired(false);

        //public Guid? OldSystemSkuId { get; set; } = null;
        entity.Property(m => m.OldSystemSkuId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required]
        //public int Ordinal { get; set; } = default!;
        entity.Property(m => m.Ordinal)
            .IsRequired(true);

        //[Required]
        //public short Quantity { get; set; } = default!;
        entity.Property(m => m.Quantity)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Order))]
        //public int OrderId { get; set; } = null;
        entity.Property(m => m.OrderId)
            .IsRequired(true);
        
        //[ForeignKey(nameof(Sku))]
        //public int SkuId { get; set; } = null;
        entity.Property(m => m.SkuId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public OrderDb Order { get; set; } = null;
        entity.HasOne(m => m.Order)
            .WithMany(m => m.OrderSkus)
            .HasForeignKey(m => m.OrderId)
            .IsRequired(true);

        //public SkuDb Sku { get; set; } = null;
        entity.HasOne(m => m.Sku)
            .WithMany(m => m.OrderSkus)
            .HasForeignKey(m => m.SkuId)
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemOrderSkuId)
            .IsUnique(true);
        #endregion
    }
}