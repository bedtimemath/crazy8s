using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class KitConfig : BaseCoreConfig<KitDb>
{
    public override void Configure(EntityTypeBuilder<KitDb> entity)
    {
        #region Id Property
        // [Required]
        // public int KitId { get; set; }
        entity.HasKey(m => m.KitId);
        #endregion

        #region Computed Properties
        //[MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //public string Key { get; set; } = null!;
        entity.Property(e => e.Key)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasComputedColumnSql("CASE WHEN [Version] IS NOT NULL THEN 'C8.S' " +
              "+ CAST([Season] AS VARCHAR) + '.' + [Year] + '.' + [Version] + '.' + RIGHT([AgeLevel],2) " +
              "ELSE 'C8.S' + CAST([Season] AS VARCHAR) + '.' + [Year] + '.' + RIGHT([AgeLevel],2) END")
            .IsRequired(true);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public KitStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
        //public string Year { get; set; }
        entity.Property(m => m.Year)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(true);

        //public int Season { get; set; }
        entity.Property(m => m.Season)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Shorter)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public AgeLevel AgeLevel { get; set; }
        entity.Property(m => m.AgeLevel)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Shorter)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
        //public string? Version { get; set; } = null;
        entity.Property(m => m.Version)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Comments)]
        //public string? Comments { get; set; }
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Comments)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[Required, ForeignKey(nameof(Offer))]
        //public int OfferId { get; set; }
        entity.Property(m => m.OfferId)
            .IsRequired(true);

        //[Required, ForeignKey(nameof(KitPage))]
        //public int? KitPageId { get; set; }
        entity.Property(m => m.KitPageId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public OfferDb Offer { get; set; } = null!;
        entity.HasOne<OfferDb>(m => m.Offer)
            .WithMany(m => m.Kits)
            .HasForeignKey(m => m.OfferId)
            .IsRequired(true);

        //public KitPageDb? KitPage { get; set; } = null!;
        entity.HasOne<KitPageDb>()
            .WithMany(m => m.Kits)
            .HasForeignKey(m => m.KitPageId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = null!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Kit)
            .HasForeignKey(m => m.KitId);
        #endregion

        #region Indices
        entity.HasIndex(m => new { m.Year, m.Season, m.AgeLevel, m.Version })
            .IsUnique(true);
        #endregion
    }
}