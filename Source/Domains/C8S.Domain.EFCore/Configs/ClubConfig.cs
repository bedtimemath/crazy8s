using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class ClubConfig : BaseCoreConfig<ClubDb>
{
    public override void Configure(EntityTypeBuilder<ClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ClubId { get; set; }
        entity.HasKey(m => m.ClubId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemClubId { get; set; }
        entity.Property(m => m.OldSystemClubId)
            .IsRequired(false);

        //public Guid? OldSystemOrganizationId { get; set; }
        entity.Property(m => m.OldSystemOrganizationId)
            .IsRequired(false);

        //public Guid? OldSystemCoachId { get; set; }
        entity.Property(m => m.OldSystemCoachId)
            .IsRequired(false);

        //public Guid? OldSystemMeetingAddressId { get; set; }
        entity.Property(m => m.OldSystemMeetingAddressId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ClubStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //public string? Year { get; set; }
        entity.Property(m => m.Year)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //public int Season { get; set; } = default!;
        entity.Property(m => m.Season)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public AgeLevel? AgeLevel { get; set; } = default!;
        entity.Property(m => m.AgeLevel)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ClubSize? ClubSize { get; set; } = default!;
        entity.Property(m => m.ClubSize)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //public DateOnly? StartsOn { get; set; }
        entity.Property(m => m.StartsOn)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[Required, ForeignKey(nameof(Place))]
        //public int PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(true);

        //[ForeignKey(nameof(Sale))]
        //public int? SaleId { get; set; } = default!;
        entity.Property(m => m.SaleId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(true);

        //public SaleDb? Sale { get; set; }
        entity.HasOne(m => m.Sale)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.SaleId)
            .IsRequired(false);

        //public ICollection<ClubPersonDb> ClubPersons { get; set; } = default!;
        entity.HasMany(m => m.ClubPersons)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(false);

        //public ICollection<OrderDb> Orders { get; set; } = null!;
        entity.HasMany(m => m.Orders)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(false);

        //public ICollection<ClubNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemClubId)
            .IsUnique(true);
        #endregion
    }
}