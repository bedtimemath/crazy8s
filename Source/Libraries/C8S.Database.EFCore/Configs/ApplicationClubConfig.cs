using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class ApplicationClubConfig : IEntityTypeConfiguration<ApplicationClubDb>
{
    public void Configure(EntityTypeBuilder<ApplicationClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ApplicationClubId { get; set; }
        entity.HasKey(m => m.ApplicationClubId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemApplicationClubId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationClubId)
            .IsRequired(false);

        //public Guid? OldSystemApplicationId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedClubId { get; set; } = null;
        entity.Property(m => m.OldSystemLinkedClubId)
            .IsRequired(false);

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

        //[Required]
        //public int Season { get; set; } = default!;
        entity.Property(m => m.Season)
            .IsRequired(true);

        //[Required]
        //public DateOnly StartsOn { get; set; }
        entity.Property(m => m.StartsOn)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Application))]
        //public int ApplicationId { get; set; } = default!;
        entity.Property(m => m.ApplicationId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public ApplicationDb Application { get; set; } = default!;
        entity.HasOne(m => m.Application)
            .WithMany(m => m.ApplicationClubs)
            .HasForeignKey(m => m.ApplicationId)
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemApplicationClubId)
            .IsUnique(true);
        #endregion
    }
}