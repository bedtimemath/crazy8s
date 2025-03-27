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

        //[MaxLength(SharedConstants.MaxLengths.Description)]
        //public string? Description { get; set; }
        entity.Property(m => m.Description)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Description);
        #endregion

        #region Navigation Configuration
        //public KitDb Kit { get; set; } = null!;
        entity.HasOne<KitDb>(m => m.Kit)
            .WithOne(m => m.Offer)
            .HasForeignKey<KitDb>(m => m.OfferId)
            .IsRequired(true);
        #endregion
    }
}