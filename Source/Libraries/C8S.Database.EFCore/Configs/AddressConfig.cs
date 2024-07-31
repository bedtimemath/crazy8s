using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class AddressConfig : IEntityTypeConfiguration<AddressDb>
{
    public void Configure(EntityTypeBuilder<AddressDb> entity)
    {
        #region Id Property
        // [Required]
        // public int AddressId { get; set; }
        entity.HasKey(m => m.AddressId);
        #endregion

        #region Database Properties
        //public Guid? OldSystemUsaPostalId { get; set; } = null;
        entity.Property(m => m.OldSystemUsaPostalId)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? RecipientName { get; set; } = null;
        entity.Property(m => m.RecipientName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? BusinessName { get; set; } = default!;
        entity.Property(m => m.BusinessName)
            .HasMaxLength(SharedConstants.MaxLengths.Name)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string StreetAddress { get; set; } = default!;
        entity.Property(m => m.StreetAddress)
            .HasMaxLength(SharedConstants.MaxLengths.Standard)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string City { get; set; } = default!;
        entity.Property(m => m.City)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Tiny)]
        //public string State { get; set; } = default!;
        entity.Property(m => m.State)
            .HasMaxLength(SharedConstants.MaxLengths.Tiny)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.ZIPCode)]
        //public string PostalCode { get; set; } = default!;
        entity.Property(m => m.PostalCode)
            .HasMaxLength(SharedConstants.MaxLengths.ZIPCode)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string TimeZone { get; set; } = default!;
        entity.Property(m => m.TimeZone)
            .HasMaxLength(SharedConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required]
        //public bool IsMilitary { get; set; } = default!;
        entity.Property(m => m.IsMilitary)
            .HasDefaultValue(false)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public OrganizationDb Organization { get; set; } = default!;
        entity.HasOne(m => m.Organization)
            .WithOne(m => m.Address)
            .HasForeignKey<OrganizationDb>(m => m.AddressId)
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemUsaPostalId)
            .IsUnique(true);
        #endregion
    }
}