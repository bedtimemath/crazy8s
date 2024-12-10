using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class InvoicePersonConfig : IEntityTypeConfiguration<InvoicePersonDb>
{
    public void Configure(EntityTypeBuilder<InvoicePersonDb> entity)
    {
        #region Id Property
        // [Required]
        // public int InvoicePersonId { get; set; }
        entity.HasKey(m => m.InvoicePersonId);
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
        
        //[ForeignKey(nameof(Invoice))]
        //public int InvoiceId { get; set; } = null;
        entity.Property(m => m.InvoiceId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.InvoicePersons)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public InvoiceDb Invoice { get; set; } = null;
        entity.HasOne(m => m.Invoice)
            .WithMany(m => m.InvoicePersons)
            .HasForeignKey(m => m.InvoiceId)
            .IsRequired(true);
        #endregion
    }
}