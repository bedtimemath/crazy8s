using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class SkuConfig : BaseCoreConfig<SkuDb>
{
    public override void Configure(EntityTypeBuilder<SkuDb> entity)
    {
        #region Id Property
        // [Required]
        // public int SkuId { get; set; }
        entity.HasKey(m => m.SkuId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemSkuId { get; set; } = null;
        entity.Property(m => m.OldSystemSkuId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SharedConstants.MaxLengths.Key)]
        //public string Key { get; set; } = default!;
        entity.Property(m => m.Key)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Key)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string Name { get; set; } = default!;
        entity.Property(m => m.Name)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public SkuStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //public int? Season { get; set; }
        entity.Property(m => m.Season)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public AgeLevel? AgeLevel { get; set; }
        entity.Property(m => m.AgeLevel)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ClubSize? ClubSize { get; set; }
        entity.Property(m => m.ClubSize)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XLong)]
        //public string? Comments { get; set; }
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XLong);
        #endregion

        #region Navigation Configuration
        //public ICollection<OrderSkuDb> OrderSkus { get; set; } = default!;
        entity.HasMany(m => m.OrderSkus)
            .WithOne(m => m.Sku)
            .HasForeignKey(m => m.SkuId);

        //public ICollection<PermissionDb> Permissions { get; set; } = default!;
        entity.HasMany(m => m.Permissions)
            .WithOne(m => m.Sku)
            .HasForeignKey(m => m.SkuId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemSkuId)
            .IsUnique(true);
        #endregion
    }
}