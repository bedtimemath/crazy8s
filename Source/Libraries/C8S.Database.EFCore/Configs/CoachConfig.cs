using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class CoachConfig : IEntityTypeConfiguration<CoachDb>
{
    public void Configure(EntityTypeBuilder<CoachDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.CoachId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemCoachId { get; set; } = null;
        entity.Property(m => m.OldSystemCoachId)
            .IsRequired(false);

        //public Guid? OldSystemOrganizationId { get; set; } = null;
        entity.Property(m => m.OldSystemOrganizationId)
            .IsRequired(false);

        //public Guid? OldSystemUserId { get; set; } = null;
        entity.Property(m => m.OldSystemUserId)
            .IsRequired(false);

        //public Guid? OldSystemCompanyId { get; set; } = null;
        entity.Property(m => m.OldSystemCompanyId)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string FirstName { get; set; } = default!;
        entity.Property(m => m.FirstName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string LastName { get; set; } = default!;
        entity.Property(m => m.LastName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Email)]
        //public string Email { get; set; } = default!;
        entity.Property(m => m.Email)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string TimeZone { get; set; } = default!;
        entity.Property(m => m.TimeZone)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? Phone { get; set; } = null;
        entity.Property(m => m.Phone)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? PhoneExt { get; set; } = null;
        entity.Property(m => m.PhoneExt)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Organization))]
        //public int? OrganizationId { get; set; } = default!;
        entity.Property(m => m.OrganizationId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public OrganizationDb? Organization { get; set; } = default!;
        entity.HasOne(m => m.Organization)
            .WithMany(m => m.Coaches)
            .HasForeignKey(m => m.OrganizationId)
            .IsRequired(false);

        //public ICollection<ApplicationDb> Applications { get; set; } = default!;
        entity.HasMany(m => m.Applications)
            .WithOne(m => m.LinkedCoach)
            .HasForeignKey(m => m.LinkedCoachId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = default!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Coach)
            .HasForeignKey(m => m.CoachId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemCoachId)
            .IsUnique(true);
        #endregion
    }
}