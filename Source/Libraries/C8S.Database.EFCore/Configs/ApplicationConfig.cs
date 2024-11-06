using C8S.Database.EFCore.Base;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Database.EFCore.Configs;

public class ApplicationConfig : BaseConfig<ApplicationDb>
{
    public override void Configure(EntityTypeBuilder<ApplicationDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ApplicationId { get; set; }
        entity.HasKey(m => m.ApplicationId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemApplicationId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationId)
            .IsRequired(false);

        //public Guid? OldSystemAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemAddressId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedCoachId { get; set; } = null;
        entity.Property(m => m.OldSystemLinkedCoachId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedOrganizationId { get; set; } = null;
        entity.Property(m => m.OldSystemLinkedOrganizationId)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ApplicationStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ApplicantType? ApplicantType { get; set; } = default!;
        entity.Property(m => m.ApplicantType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? ApplicantFirstName { get; set; } = default!;
        entity.Property(m => m.ApplicantFirstName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string ApplicantLastName { get; set; } = default!;
        entity.Property(m => m.ApplicantLastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Email)]
        //public string ApplicantEmail { get; set; } = default!;
        entity.Property(m => m.ApplicantEmail)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? ApplicantPhone { get; set; } = default!;
        entity.Property(m => m.ApplicantPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? ApplicantPhoneExt { get; set; } = default!;
        entity.Property(m => m.ApplicantPhoneExt)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string ApplicantTimeZone { get; set; } = default!;
        entity.Property(m => m.ApplicantTimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string? OrganizationName { get; set; } = null;
        entity.Property(m => m.OrganizationName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType? OrganizationType { get; set; } = null;
        entity.Property(m => m.OrganizationType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? OrganizationTypeOther { get; set; } = null;
        entity.Property(m => m.OrganizationTypeOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? OrganizationTaxIdentifier { get; set; } = null;
        entity.Property(m => m.OrganizationTaxIdentifier)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; } = null;
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);

        //[Required]
        //public DateTimeOffset SubmittedOn { get; set; }
        entity.Property(m => m.SubmittedOn)
            .IsRequired(true);

        //[Required]
        //public bool IsCoachRemoved { get; set; } = false;
        entity.Property(m => m.IsCoachRemoved)
            .HasDefaultValue(false)
            .IsRequired(true);

        //[Required]
        //public bool IsOrganizationRemoved { get; set; } = false;
        entity.Property(m => m.IsOrganizationRemoved)
            .HasDefaultValue(false)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Address))]
        //public int? AddressId { get; set; } = default!;
        entity.Property(m => m.AddressId)
            .IsRequired(false);

        //[ForeignKey(nameof(LinkedCoach))]
        //public int? LinkedCoachId { get; set; } = default!;
        entity.Property(m => m.LinkedCoachId)
            .IsRequired(false);

        //[ForeignKey(nameof(LinkedOrganization))]
        //public int? LinkedOrganizationId { get; set; } = default!;
        entity.Property(m => m.LinkedOrganizationId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public AddressDb? Address { get; set; } = default!;
        entity.HasOne(m => m.Address)
            .WithOne(m => m.Application)
            .HasForeignKey<ApplicationDb>(m => m.AddressId)
            .IsRequired(false);

        //public CoachDb? LinkedCoach { get; set; } = default!;
        entity.HasOne(m => m.LinkedCoach)
            .WithMany(m => m.Applications)
            .HasForeignKey(m => m.LinkedCoachId)
            .IsRequired(false);

        //public OrganizationDb? LinkedOrganization { get; set; } = default!;
        entity.HasOne(m => m.LinkedOrganization)
            .WithMany(m => m.Applications)
            .HasForeignKey(m => m.LinkedOrganizationId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemApplicationId)
            .IsUnique(true);
        #endregion

        #region Auto-Includes
        entity.Navigation(m => m.ApplicationClubs)
            .AutoInclude();
        entity.Navigation(m => m.Address)
            .AutoInclude();
        #endregion
    }
}