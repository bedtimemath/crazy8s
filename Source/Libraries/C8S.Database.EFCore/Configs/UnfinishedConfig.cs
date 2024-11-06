using C8S.Database.EFCore.Base;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Database.EFCore.Configs;

public class UnfinishedConfig : BaseConfig<UnfinishedDb>
{
    public override void Configure(EntityTypeBuilder<UnfinishedDb> entity)
    {
        #region Id Property
        // [Required]
        // public int UnfinishedId { get; set; }
        entity.HasKey(m => m.UnfinishedId);
        #endregion

        #region Database Properties
        //public Guid? Code { get; set; } = Guid.NewGuid();
        entity.Property(m => m.Code)
            .HasDefaultValueSql("NEWID()")
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

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string ApplicantLastName { get; set; } = default!;
        entity.Property(m => m.ApplicantLastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Email)]
        //public string ApplicantEmail { get; set; } = default!;
        entity.Property(m => m.ApplicantEmail)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? ApplicantPhone { get; set; } = default!;
        entity.Property(m => m.ApplicantPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string ApplicantTimeZone { get; set; } = default!;
        entity.Property(m => m.ApplicantTimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public bool? HasHostedBefore { get; set; } = null;
        entity.Property(m => m.HasHostedBefore)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string? OrganizationName { get; set; } = null;
        entity.Property(m => m.OrganizationName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? OrganizationAddress1 { get; set; } = default!;
        entity.Property(m => m.OrganizationAddress1)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? OrganizationAddress2 { get; set; } = default!;
        entity.Property(m => m.OrganizationAddress2)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? OrganizationCity { get; set; } = default!;
        entity.Property(m => m.OrganizationCity)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Tiny)]
        //public string? State { get; set; } = default!;
        entity.Property(m => m.OrganizationState)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.ZIPCode)]
        //public string? PostalCode { get; set; } = default!;
        entity.Property(m => m.OrganizationPostalCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.ZIPCode)
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

        //public string? ClubsString { get; set; } = null;
        entity.Property(m => m.ClubsString)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XLong)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; } = null;
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public DateTimeOffset? ChosenTimeSlot { get; set; } = null;
        entity.Property(m => m.ChosenTimeSlot)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? ReferenceSource { get; set; } = null;
        entity.Property(m => m.ReferenceSource)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Long)]
        //public string? ReferenceSourceOther { get; set; } = null;
        entity.Property(m => m.ReferenceSourceOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Long)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; } = null;
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);

        //public DateTimeOffset? EndPart01On { get; set; }
        entity.Property(m => m.EndPart01On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart02On { get; set; }
        entity.Property(m => m.EndPart02On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart03On { get; set; }
        entity.Property(m => m.EndPart03On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart04On { get; set; }
        entity.Property(m => m.EndPart04On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart05On { get; set; }
        entity.Property(m => m.EndPart05On)
            .IsRequired(false);

        //public DateTimeOffset? SubmittedOn { get; set; }
        entity.Property(m => m.SubmittedOn)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Code)
            .IsUnique(true);
        #endregion
    }
}