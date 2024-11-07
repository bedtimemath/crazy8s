using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class CoachConfig : BaseConfig<CoachDb>
{
    public override void Configure(EntityTypeBuilder<CoachDb> entity)
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
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string LastName { get; set; } = default!;
        entity.Property(m => m.LastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Email)]
        //public string Email { get; set; } = default!;
        entity.Property(m => m.Email)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string TimeZone { get; set; } = default!;
        entity.Property(m => m.TimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? Phone { get; set; } = null;
        entity.Property(m => m.Phone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? PhoneExt { get; set; } = null;
        entity.Property(m => m.PhoneExt)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
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