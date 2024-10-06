using C8S.Database.Abstractions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Base;

public abstract class BaseConfig<TCoreDb>: IEntityTypeConfiguration<TCoreDb>
where TCoreDb: class, IBaseDb
{
    public virtual void Configure(EntityTypeBuilder<TCoreDb> entity)
    {
        #region Database Properties
        // [Required]
        // public DateTimeOffset? CreatedOn { get; set; } = default!;
        entity.Property(m => m.CreatedOn)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .IsRequired(true);
        #endregion
    }
}