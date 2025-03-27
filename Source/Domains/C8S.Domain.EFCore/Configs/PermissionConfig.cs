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
        //public DateTimeOffset? ExpiresOn { get; set; }
        entity.Property(m => m.ExpiresOn)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Person))]
        //public int PersonId { get; set; } = null;
        entity.Property(m => m.PersonId)
            .IsRequired(true);

        //[ForeignKey(nameof(KitPage))]
        //public int KitPageId { get; set; }
        entity.Property(m => m.KitPageId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.Permissions)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public KitPageDb KitPage { get; set; } = null!;
        entity.HasOne(m => m.KitPage)
            .WithMany(m => m.Permissions)
            .HasForeignKey(m => m.KitPageId)
            .IsRequired(true);
        #endregion
    }
}