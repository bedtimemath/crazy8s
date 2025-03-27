using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class OldNewConfig : IEntityTypeConfiguration<OldNewDb>
{
    public void Configure(EntityTypeBuilder<OldNewDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OldNewId { get; set; }
        entity.HasKey(m => m.OldNewId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //public string OldTableName { get; set; } = null!;
        entity.Property(m => m.OldTableName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required]
        //public Guid OldId { get; set; }
        entity.Property(m => m.OldId)
            .IsRequired(true);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //public string NewTableName { get; set; } = null!;
        entity.Property(m => m.NewTableName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required]
        //public int NewId { get; set; }
        entity.Property(m => m.NewId)
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => new { m.OldTableName, m.OldId }).IsUnique(false);
        entity.HasIndex(m => new { m.NewTableName, m.NewId }).IsUnique(false);
        #endregion
    }
}