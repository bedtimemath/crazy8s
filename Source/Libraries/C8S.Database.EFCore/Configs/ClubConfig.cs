using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class ClubConfig : IEntityTypeConfiguration<ClubDb>
{
    public void Configure(EntityTypeBuilder<ClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ClubId { get; set; }
        entity.HasKey(m => m.ClubId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemClubId { get; set; } = null;
        entity.Property(m => m.OldSystemClubId)
            .IsRequired(false);

        //public Guid? OldSystemOrganizationId { get; set; } = null;
        entity.Property(m => m.OldSystemOrganizationId)
            .IsRequired(false);

        //public Guid? OldSystemCoachId { get; set; } = null;
        entity.Property(m => m.OldSystemCoachId)
            .IsRequired(false);

        //public Guid? OldSystemMeetingAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemMeetingAddressId)
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

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Coach))]
        //public int CoachId { get; set; } = default!;
        entity.Property(m => m.CoachId)
            .IsRequired(true);

        //[ForeignKey(nameof(Organization))]
        //public int OrganizationId { get; set; } = default!;
        entity.Property(m => m.OrganizationId)
            .IsRequired(true);

        //[ForeignKey(nameof(Address))]
        //public int? AddressId { get; set; } = default!;
        entity.Property(m => m.AddressId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public CoachDb Coach { get; set; } = default!;
        entity.HasOne(m => m.Coach)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.CoachId)
            .IsRequired(true);

        //public OrganizationDb Organization { get; set; } = default!;
        entity.HasOne(m => m.Organization)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.OrganizationId)
            .IsRequired(true);

        //public AddressDb? Address { get; set; } = null;
        entity.HasOne(m => m.Address)
            .WithOne(m => m.Club)
            .HasForeignKey<ClubDb>(m => m.AddressId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemClubId)
            .IsUnique(true);
        #endregion

        #region Auto-Includes
        entity.Navigation(m => m.Address)
            .AutoInclude();
        #endregion
    }
}