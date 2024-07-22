using C8S.Common;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Database.EFCore.Configs;

public class CoachConfig : IEntityTypeConfiguration<CoachDb>
{
    public void Configure(EntityTypeBuilder<CoachDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.CoachId);
        #endregion

        #region Database Properties
        // [Required, MaxLength(SharedConstants.MaxLengths.FullName)]
        // public string Name { get; set; } = default!;
        entity.Property(m => m.Name)
            .IsRequired(true);

        // [Required, MaxLength(SharedConstants.MaxLengths.Email)]
        // public string? Email { get; set; } = default!;
        entity.Property(m => m.Email)
            .IsRequired(true);

        // [Required, MaxLength(SharedConstants.MaxLengths.Phone)]
        // public string Phone { get; set; } = default!;
        entity.Property(m => m.Phone)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public CoachStatus Status { get; set; } = CoachStatus.Pending;
        entity.Property(m => m.Status)
            .HasConversion<string>()
            .HasMaxLength(SharedConstants.MaxLengths.Short)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Url)]
        //public string Image { get; set; } = null;
        entity.Property(m => m.Image)
            .HasMaxLength(SharedConstants.MaxLengths.Url)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Long)]
        //public string? TagLine { get; set; } = null;
        entity.Property(m => m.TagLine)
            .HasMaxLength(SharedConstants.MaxLengths.Long)
            .IsRequired(false);

        //public string? Bio { get; set; } = null;
        entity.Property(m => m.Bio)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? AuthId { get; set; } = null;
        entity.Property(m => m.AuthId)
            .HasMaxLength(SharedConstants.MaxLengths.Standard)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Group))]
        //public int? GroupId { get; set; } = default!;
        //entity.Property(m => m.GroupId)
        //    .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public GroupDb? Group { get; set; } = default!;
        //entity.HasOne(m => m.Group)
        //    .WithMany(m => m.Coachs)
        //    .HasForeignKey(m => m.GroupId)
        //    .IsRequired(false);

        //public ICollection<LeadDb> Leads { get; set; } = default!;
        //entity.HasMany(m => m.Leads)
        //    .WithOne(m => m.Coach)
        //    .HasForeignKey(m => m.CoachId);

        //public ICollection<ActionDb> Actions { get; set; } = default!;
        //entity.HasMany(m => m.Actions)
        //    .WithOne(m => m.Coach)
        //    .HasForeignKey(m => m.CoachId);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Email)
            .IsUnique(true);
        #endregion
    }
}