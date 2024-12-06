using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class PersonClubConfig : BaseConfig<PersonClubDb>
{
    public override void Configure(EntityTypeBuilder<PersonClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int PersonClubId { get; set; }
        entity.HasKey(m => m.PersonClubId);
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
            .WithMany(m => m.PersonClubs)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public ClubDb Club { get; set; } = null;
        entity.HasOne(m => m.Club)
            .WithMany(m => m.PersonClubs)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(true);
        #endregion
    }
}