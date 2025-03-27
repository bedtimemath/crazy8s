using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class KitPageConfig : BaseCoreConfig<KitPageDb>
{
    public override void Configure(EntityTypeBuilder<KitPageDb> entity)
    {
        #region Id Property
        // [Required]
        // public int KitPageId { get; set; }
        entity.HasKey(m => m.KitPageId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public KitPageStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Url)]
        //public string Url { get; set; } = null!;
        entity.Property(m => m.Url)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Url)
            .IsRequired(true);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Title)]
        //public string Title { get; set; } = null!;
        entity.Property(m => m.Title)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Title)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public ICollection<KitDb> Kits { get; set; } = null!;
        entity.HasMany(m => m.Kits)
            .WithOne(m => m.KitPage)
            .HasForeignKey(m => m.KitPageId)
            .IsRequired(false);

        //public ICollection<PermissionDb> Permissions { get; set; } = null!;
        entity.HasMany(m => m.Permissions)
            .WithOne(m => m.KitPage)
            .HasForeignKey(m => m.KitPageId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Url)
            .IsUnique(true);
        #endregion
    }
}