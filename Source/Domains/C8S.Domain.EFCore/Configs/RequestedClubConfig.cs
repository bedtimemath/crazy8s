using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class RequestedClubConfig : IEntityTypeConfiguration<RequestedClubDb>
{
    public void Configure(EntityTypeBuilder<RequestedClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int RequestedClubId { get; set; }
        entity.HasKey(m => m.RequestedClubId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemApplicationClubId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationClubId)
            .IsRequired(false);

        //public Guid? OldSystemApplicationId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedClubId { get; set; } = null;
        entity.Property(m => m.OldSystemLinkedClubId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required]
        //public int Season { get; set; } = default!;
        entity.Property(m => m.Season)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public AgeLevel AgeLevel { get; set; } = default!;
        entity.Property(m => m.AgeLevel)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required]
        //public DateOnly StartsOn { get; set; }
        entity.Property(m => m.StartsOn)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Request))]
        //public int RequestId { get; set; } = default!;
        entity.Property(m => m.RequestId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public RequestDb Request { get; set; } = default!;
        entity.HasOne(m => m.Request)
            .WithMany(m => m.RequestedClubs)
            .HasForeignKey(m => m.RequestId)
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemApplicationClubId)
            .IsUnique(true);
        #endregion
    }
}