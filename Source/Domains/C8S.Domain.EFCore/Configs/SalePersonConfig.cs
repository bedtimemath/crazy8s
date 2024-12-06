using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class SalePersonConfig : BaseConfig<SalePersonDb>
{
    public override void Configure(EntityTypeBuilder<SalePersonDb> entity)
    {
        #region Id Property
        // [Required]
        // public int SalePersonId { get; set; }
        entity.HasKey(m => m.SalePersonId);
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
        
        //[ForeignKey(nameof(Sale))]
        //public int SaleId { get; set; } = null;
        entity.Property(m => m.SaleId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.SalePersons)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public SaleDb Sale { get; set; } = null;
        entity.HasOne(m => m.Sale)
            .WithMany(m => m.SalePersons)
            .HasForeignKey(m => m.SaleId)
            .IsRequired(true);
        #endregion
    }
}