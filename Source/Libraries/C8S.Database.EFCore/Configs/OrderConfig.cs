using C8S.Database.EFCore.Base;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Database.EFCore.Configs;

public class OrderConfig : BaseConfig<OrderDb>
{
    public override void Configure(EntityTypeBuilder<OrderDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrderId { get; set; }
        entity.HasKey(m => m.OrderId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemOrderId { get; set; } = null;
        entity.Property(m => m.OldSystemOrderId)
            .IsRequired(false);

        //public Guid? OldSystemShippingAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemShippingAddressId)
            .IsRequired(false);

        //public Guid? OldSystemClubId { get; set; } = null;
        entity.Property(m => m.OldSystemClubId)
            .IsRequired(false);

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

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? ContactPhoneExt { get; set; } = null;
        entity.Property(m => m.ContactPhoneExt)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[Required]
        //public DateTimeOffset OrderedOn { get; set; } = default!;
        entity.Property(m => m.OrderedOn)
            .IsRequired(true);

        //[Required]
        //public DateOnly ArriveBy { get; set; } = default!;
        entity.Property(m => m.ArriveBy)
            .IsRequired(true);

        //public DateTimeOffset? ShippedOn { get; set; } = default!;
        entity.Property(m => m.ShippedOn)
            .IsRequired(false);

        //public DateTimeOffset? EmailedOn { get; set; } = default!;
        entity.Property(m => m.ShippedOn)
            .IsRequired(false);

        //public Guid? BatchIdentifier { get; set; } = default!;
        entity.Property(m => m.BatchIdentifier)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Address))]
        //public int AddressId { get; set; } = null;
        entity.Property(m => m.AddressId)
            .IsRequired(true);
        
        //[ForeignKey(nameof(Club))]
        //public int? ClubId { get; set; } = null;
        entity.Property(m => m.ClubId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public AddressDb? Address { get; set; } = null;
        entity.HasOne(m => m.Address)
            .WithOne(m => m.Order)
            .HasForeignKey<OrderDb>(m => m.AddressId)
            .IsRequired(false);

        //public ClubDb? Club { get; set; } = null;
        entity.HasOne(m => m.Club)
            .WithMany(m => m.Orders)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(false);
        
        //public ICollection<OrderSkuDb> OrderSkus { get; set; } = default!;
        entity.HasMany(m => m.OrderSkus)
            .WithOne(m => m.Order)
            .HasForeignKey(m => m.OrderId);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemOrderId)
            .IsUnique(true);
        #endregion

        #region Auto-Includes
        entity.Navigation(m => m.Address)
            .AutoInclude();
        #endregion
    }
}