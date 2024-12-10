using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class ShipmentConfig : IEntityTypeConfiguration<ShipmentDb>
{
    public void Configure(EntityTypeBuilder<ShipmentDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ShipmentId { get; set; }
        entity.HasKey(m => m.ShipmentId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
        //public string TrackingNumber { get; set; } = default!;
        entity.Property(m => m.TrackingNumber)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Key)
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ShipMethod? ShipMethod { get; set; } = default!;
        entity.Property(m => m.ShipMethod)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Medium)]
        //public string? ShipMethodOther { get; set; } = null;
        entity.Property(m => m.ShipMethodOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Order))]
        //public int? OrderId { get; set; } = default!;
        entity.Property(m => m.OrderId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public OrderDb? Order { get; set; } = default!;
        entity.HasOne(m => m.Order)
            .WithMany(m => m.Shipments)
            .HasForeignKey(m => m.OrderId)
            .IsRequired(false);
        #endregion
    }
}