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
        
        //public Guid? OldSystemPostalAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemPostalAddressId)
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
        //public string? Notes { get; set; } = null;
        entity.Property(m => m.Notes)
            .HasMaxLength(SharedConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Address))]
        //public int? AddressId { get; set; } = null;
        entity.Property(m => m.AddressId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public AddressDb? Address { get; set; } = null;
        entity.HasOne(m => m.Address)
            .WithOne(m => m.Organization)
            .HasForeignKey<OrganizationDb>(m => m.AddressId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = default!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Organization)
            .HasForeignKey(m => m.OrganizationId);

        //public ICollection<CoachDb> Coaches { get; set; } = default!;
        entity.HasMany(m => m.Coaches)
            .WithOne(m => m.Organization)
            .HasForeignKey(m => m.OrganizationId);

        //public ICollection<ApplicationDb> Applications { get; set; } = default!;
        entity.HasMany(m => m.Applications)
            .WithOne(m => m.LinkedOrganization)
            .HasForeignKey(m => m.LinkedOrganizationId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemOrganizationId)
            .IsUnique(true);
        #endregion

        #region Auto-Includes
        entity.Navigation(m => m.Address)
            .AutoInclude();
        #endregion
    }
}