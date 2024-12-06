using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class SaleConfig : BaseConfig<SaleDb>
{
    public override void Configure(EntityTypeBuilder<SaleDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.SaleId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public SaleStatus Status { get; set; } = SaleStatus.Potential;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Place))]
        //public int? PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb? Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Sales)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);
        #endregion
    }
}