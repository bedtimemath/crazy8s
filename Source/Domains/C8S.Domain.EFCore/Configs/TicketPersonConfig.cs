using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class TicketPersonConfig : IEntityTypeConfiguration<TicketPersonDb>
{
    public void Configure(EntityTypeBuilder<TicketPersonDb> entity)
    {
        #region Id Property
        // [Required]
        // public int SalePersonId { get; set; }
        entity.HasKey(m => m.TicketPersonId);
        #endregion

        #region Database Properties
        //[Required]
        //public int Ordinal { get; set; }
        entity.Property(m => m.Ordinal)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Person))]
        //public int PersonId { get; set; } = null;
        entity.Property(m => m.PersonId)
            .IsRequired(true);

        //[ForeignKey(nameof(Sale))]
        //public int SaleId { get; set; } = null;
        entity.Property(m => m.TicketId)
            .IsRequired(true);
        #endregion

        #region Navigation Configuration
        //public PersonDb Person { get; set; } = null;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.TicketPersons)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(true);

        //public TicketDb Ticket { get; set; } = null;
        entity.HasOne(m => m.Ticket)
            .WithMany(m => m.TicketPersons)
            .HasForeignKey(m => m.TicketId)
            .IsRequired(true);
        #endregion
    }
}