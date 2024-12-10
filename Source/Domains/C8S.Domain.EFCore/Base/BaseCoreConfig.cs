using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Base;

public abstract class BaseCoreConfig<TCoreDb> : IEntityTypeConfiguration<TCoreDb>
where TCoreDb : class, ICoreDb
{
    public virtual void Configure(EntityTypeBuilder<TCoreDb> entity)
    {
        #region Database Properties
        // [Required]
        // public DateTimeOffset? CreatedOn { get; set; } = default!;
        entity.Property(m => m.CreatedOn)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .IsRequired(true);

        // public DateTimeOffset? ModifiedOn { get; set; }
        entity.Property(m => m.ModifiedOn)
            .IsRequired(false);
        #endregion
    }
}