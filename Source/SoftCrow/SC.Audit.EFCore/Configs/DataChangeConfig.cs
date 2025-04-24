using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Audit.EFCore.Models;
using SC.Common;

namespace SC.Audit.EFCore.Configs;

public class DataChangeConfig : IEntityTypeConfiguration<DataChangeDb>
{
    public void Configure(EntityTypeBuilder<DataChangeDb> entity)
    {
        #region Id Property
        // [Required]
        // public int DataChangeId { get; set; }
        entity.HasKey(m => m.DataChangeId);
        #endregion

        #region Database Properties
        //public Guid? Identifier { get; set; }
        entity.Property(m => m.Identifier)
            .IsRequired(false);

        //public int? EntityId { get; set; }
        entity.Property(m => m.EntityId)
            .IsRequired(false);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string EntityName { get; set; } = default!;
        entity.Property(m => m.EntityName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(true);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public EntityState EntityState { get; set; }
        entity.Property(m => m.EntityState)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
        //public string? PropertiesJson { get; set; } = default!;
        entity.Property(m => m.PropertiesJson)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string? Description { get; set; } = default!;
        entity.Property(m => m.Description)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[Required]
        //public DateTimeOffset CreatedOn { get; set; }
        entity.Property(m => m.CreatedOn)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .IsRequired(true);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Identifier).IsUnique(false);
        entity.HasIndex(m => m.EntityId).IsUnique(false);
        entity.HasIndex(m => m.EntityName).IsUnique(false);
        entity.HasIndex(m => m.EntityState).IsUnique(false);
        #endregion
    }
}