using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class ClubPersonConfig : BaseConfig<ClubPersonDb>
{
    public override void Configure(EntityTypeBuilder<ClubPersonDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ClubPersonId { get; set; }
        entity.HasKey(m => m.ClubPersonId);
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
        
        //[ForeignKey(nameof(Club))]
        //public int ClubId { get; set; } = null;
        entity.Property(m => m.ClubId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.ClubPersons)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public ClubDb Club { get; set; } = null;
        entity.HasOne(m => m.Club)
            .WithMany(m => m.ClubPersons)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(true);
        #endregion
    }
}