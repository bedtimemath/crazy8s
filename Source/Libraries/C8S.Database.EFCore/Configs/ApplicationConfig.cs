﻿using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class ApplicationConfig : IEntityTypeConfiguration<ApplicationDb>
{
    public void Configure(EntityTypeBuilder<ApplicationDb> entity)
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

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ApplicationStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ApplicantType ApplicantType { get; set; } = default!;
        entity.Property(m => m.ApplicantType)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string ApplicantFirstName { get; set; } = default!;
        entity.Property(m => m.ApplicantFirstName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string ApplicantLastName { get; set; } = default!;
        entity.Property(m => m.ApplicantLastName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Email)]
        //public string ApplicantEmail { get; set; } = default!;
        entity.Property(m => m.ApplicantEmail)
            .HasMaxLength(SharedConstants.MaxLengths.Email)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //public string ApplicantPhone { get; set; } = default!;
        entity.Property(m => m.ApplicantPhone)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //public string ApplicantPhoneExt { get; set; } = default!;
        entity.Property(m => m.ApplicantPhoneExt)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string ApplicantTimeZone { get; set; } = default!;
        entity.Property(m => m.ApplicantTimeZone)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string OrganizationName { get; set; } = default!;
        entity.Property(m => m.OrganizationName)
            .HasMaxLength(SharedConstants.MaxLengths.FullName)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType OrganizationType { get; set; } = OrganizationType.Other;
        entity.Property(m => m.OrganizationType)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? OrganizationTypeOther { get; set; } = null;
        entity.Property(m => m.OrganizationTypeOther)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? OrganizationTaxIdentifier { get; set; } = null;
        entity.Property(m => m.OrganizationTaxIdentifier)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; } = null;
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; } = null;
        entity.Property(m => m.OldSystemNotes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong)
            .IsRequired(false);

        //[Required]
        //public DateTimeOffset SubmittedOn { get; set; }
        entity.Property(m => m.SubmittedOn)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? OldSystemNotes { get; set; } = null;
        entity.Property(m => m.OldSystemNotes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
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
    }
}