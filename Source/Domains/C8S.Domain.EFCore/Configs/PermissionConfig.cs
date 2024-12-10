using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class PermissionConfig : IEntityTypeConfiguration<PermissionDb>
{
    public void Configure(EntityTypeBuilder<PermissionDb> entity)
    {
        #region Id Property
        // [Required]
        // public int PermissionId { get; set; }
        entity.HasKey(m => m.PermissionId);
        #endregion

        #region Database Properties
        //[Required]
        //public bool IsPrimary { get; set; } = default!;
        entity.Property(m => m.IsPrimary)
            .HasDefaultValue(false)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Person))]
        //public int PersonId { get; set; } = null;
        entity.Property(m => m.PersonId)
            .IsRequired(true);
        
        //[ForeignKey(nameof(Sku))]
        //public int SkuId { get; set; } = null;
        entity.Property(m => m.SkuId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.Permissions)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public SkuDb Sku { get; set; } = null;
        entity.HasOne(m => m.Sku)
            .WithMany(m => m.Permissions)
            .HasForeignKey(m => m.SkuId)
            .IsRequired(true);
        #endregion
    }
}