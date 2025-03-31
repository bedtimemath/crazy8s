using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class OfferConfig : BaseCoreConfig<OfferDb>
{
    public override void Configure(EntityTypeBuilder<OfferDb> entity)
    {
        #region Id Property
        // [Required]
        // public int SkuId { get; set; }
        entity.HasKey(m => m.OfferId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SharedConstants.MaxLengths.Key)]
        //public string FulcoId { get; set; } = default!;
        entity.Property(m => m.FulcoId)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Key)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Title)]
        //public string Title { get; set; } = default!;
        entity.Property(m => m.Title)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public SkuStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //public string Year { get; set; }
        entity.Property(m => m.Year)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(true);

        //public int Season { get; set; }
        entity.Property(m => m.Season)
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
        //public string? Version { get; set; } = null!;
        entity.Property(m => m.Version)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Description)]
        //public string? Description { get; set; }
        entity.Property(m => m.Description)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Description);
        #endregion

        #region Navigation Configuration
        //public ICollection<OrderOfferDb> OrderOffers { get; set; } = null!;
        entity.HasMany(m => m.OrderOffers)
            .WithOne(m => m.Offer)
            .HasForeignKey(m => m.OfferId);

        //public ICollection<KitDb> Kits { get; set; } = null!;
        entity.HasMany(m => m.Kits)
            .WithOne(m => m.Offer)
            .HasForeignKey(m => m.OfferId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.FulcoId)
            .IsUnique(true);
        entity.HasIndex(m => new { m.Year, m.Season, m.Version })
            .IsUnique(true);
        #endregion
    }
}