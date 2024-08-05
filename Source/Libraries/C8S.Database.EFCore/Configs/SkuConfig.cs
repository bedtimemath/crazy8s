using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class SkuConfig : IEntityTypeConfiguration<SkuDb>
{
    public void Configure(EntityTypeBuilder<SkuDb> entity)
    {
        #region Id Property
        // [Required]
        // public int SkuId { get; set; }
        entity.HasKey(m => m.SkuId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemSkuId { get; set; } = null;
        entity.Property(m => m.OldSystemSkuId)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Key)]
        //public string Key { get; set; } = default!;
        entity.Property(m => m.Key)
            .HasMaxLength(SharedConstants.MaxLengths.Key)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string Name { get; set; } = default!;
        entity.Property(m => m.Name)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required]
        //public int Season { get; set; } = default!;
        entity.Property(m => m.Season)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public SkuStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public AgeLevel AgeLevel { get; set; } = default!;
        entity.Property(m => m.AgeLevel)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ClubSize ClubSize { get; set; } = default!;
        entity.Property(m => m.ClubSize)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong);
        #endregion

        #region Navigation Configuration
        //public ICollection<OrderDb> Orders { get; set; } = default!;
        entity.HasMany(m => m.Orders)
            .WithMany(m => m.Skus)
            .UsingEntity("OrderSkus");
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemSkuId)
            .IsUnique(true);
        #endregion
    }
}