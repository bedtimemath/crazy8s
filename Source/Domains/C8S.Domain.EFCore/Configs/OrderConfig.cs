using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class OrderConfig : BaseConfig<OrderDb>
{
    public override void Configure(EntityTypeBuilder<OrderDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrderId { get; set; }
        entity.HasKey(m => m.OrderId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemOrderId { get; set; } = null;
        entity.Property(m => m.OldSystemOrderId)
            .IsRequired(false);

        //public Guid? OldSystemShippingAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemShippingAddressId)
            .IsRequired(false);

        //public Guid? OldSystemClubId { get; set; } = null;
        entity.Property(m => m.OldSystemClubId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required]
        //public int Number { get; set; } = default!;
        entity.Property(m => m.Number)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? ContactName { get; set; } = null;
        entity.Property(m => m.ContactName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Email)]
        //public string? ContactEmail { get; set; } = null;
        entity.Property(m => m.ContactEmail)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? ContactPhone { get; set; } = null;
        entity.Property(m => m.ContactPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);
        
        //[Required, MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string Recipient { get; set; } = default!;
        entity.Property(m => m.Recipient)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string StreetAddress { get; set; } = default!;
        entity.Property(m => m.Line1)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string? Line2 { get; set; } = default!;
        entity.Property(m => m.Line2)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string City { get; set; } = default!;
        entity.Property(m => m.City)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Tiny)]
        //public string State { get; set; } = default!;
        entity.Property(m => m.State)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.ZIPCode)]
        //public string PostalCode { get; set; } = default!;
        entity.Property(m => m.ZIPCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.ZIPCode)
            .IsRequired(true);

        //[Required]
        //public bool IsMilitary { get; set; } = default!;
        entity.Property(m => m.IsMilitary)
            .HasDefaultValue(false)
            .IsRequired(true);

        //[Required]
        //public DateTimeOffset OrderedOn { get; set; } = default!;
        entity.Property(m => m.OrderedOn)
            .IsRequired(true);

        //public DateOnly? ArriveBy { get; set; } = default!;
        entity.Property(m => m.ArriveBy)
            .IsRequired(false);

        //public DateTimeOffset? ShippedOn { get; set; } = default!;
        entity.Property(m => m.ShippedOn)
            .IsRequired(false);

        //public DateTimeOffset? EmailedOn { get; set; } = default!;
        entity.Property(m => m.EmailedOn)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Club))]
        //public int? ClubId { get; set; } = null;
        entity.Property(m => m.ClubId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public ClubDb? Club { get; set; } = null;
        entity.HasOne(m => m.Club)
            .WithMany(m => m.Orders)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(false);
        
        //public ICollection<ShipmentDb> Shipments { get; set; } = default!;
        entity.HasMany(m => m.Shipments)
            .WithOne(m => m.Order)
            .HasForeignKey(m => m.OrderId);
        
        //public ICollection<OrderSkuDb> OrderSkus { get; set; } = default!;
        entity.HasMany(m => m.OrderSkus)
            .WithOne(m => m.Order)
            .HasForeignKey(m => m.OrderId);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemOrderId)
            .IsUnique(true);
        #endregion
    }
}