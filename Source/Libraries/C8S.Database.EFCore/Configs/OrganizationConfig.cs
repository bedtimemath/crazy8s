using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class OrganizationConfig : IEntityTypeConfiguration<OrganizationDb>
{
    public void Configure(EntityTypeBuilder<OrganizationDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrganizationId { get; set; }
        entity.HasKey(m => m.OrganizationId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemCompanyId { get; set; } = null;
        entity.Property(m => m.OldSystemCompanyId)
            .IsRequired(false);
    
        //public Guid? OldSystemOrganizationId { get; set; } = null;
        entity.Property(m => m.OldSystemOrganizationId)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string Name { get; set; } = default!;
        entity.Property(m => m.Name)
            .HasMaxLength(SharedConstants.MaxLengths.FullName)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string TimeZone { get; set; } = default!;
        entity.Property(m => m.TimeZone)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //public string Culture { get; set; } = default!;
        entity.Property(m => m.Culture)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType Type { get; set; } = OrganizationType.Other;
        entity.Property(m => m.Type)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? TypeOther { get; set; } = null;
        entity.Property(m => m.TypeOther)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? TaxIdentifier { get; set; } = null;
        entity.Property(m => m.TaxIdentifier)
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? OldSystemNotes { get; set; } = null;
        entity.Property(m => m.OldSystemNotes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public ICollection<LeadDb> Leads { get; set; } = default!;
        entity.HasMany(m => m.Coaches)
            .WithOne(m => m.Organization)
            .HasForeignKey(m => m.OrganizationId);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemOrganizationId)
            .IsUnique(true);
        #endregion
    }
}