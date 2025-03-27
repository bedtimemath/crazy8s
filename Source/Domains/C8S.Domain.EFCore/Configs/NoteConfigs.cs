using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C8S.Domain.EFCore.Configs;

public class ClubNoteConfig : IEntityTypeConfiguration<ClubNoteDb>
{
    public void Configure(EntityTypeBuilder<ClubNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Club))]
        // public int ClubId { get; set; } = default!;
        entity.Property(m => m.ClubId)
			.IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public ClubDb Club { get; set; } = default!;
        entity.HasOne(m => m.Club)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.ClubId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}

public class InvoiceNoteConfig : IEntityTypeConfiguration<InvoiceNoteDb>
{
    public void Configure(EntityTypeBuilder<InvoiceNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Invoice))]
        // public int InvoiceId { get; set; } = default!;
        entity.Property(m => m.InvoiceId)
			.IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public InvoiceDb Invoice { get; set; } = default!;
        entity.HasOne(m => m.Invoice)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.InvoiceId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}

public class OrderNoteConfig : IEntityTypeConfiguration<OrderNoteDb>
{
    public void Configure(EntityTypeBuilder<OrderNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Order))]
        // public int OrderId { get; set; } = default!;
        entity.Property(m => m.OrderId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public OrderDb Order { get; set; } = default!;
        entity.HasOne(m => m.Order)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}

public class PersonNoteConfig : IEntityTypeConfiguration<PersonNoteDb>
{
    public void Configure(EntityTypeBuilder<PersonNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Person))]
        // public int PersonId { get; set; } = default!;
        entity.Property(m => m.PersonId)
			.IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public PersonDb Person { get; set; } = default!;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.PersonId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}

public class PlaceNoteConfig : IEntityTypeConfiguration<PlaceNoteDb>
{
    public void Configure(EntityTypeBuilder<PlaceNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Place))]
        // public int PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
			.IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public PlaceDb Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.PlaceId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}

public class TicketNoteConfig : IEntityTypeConfiguration<TicketNoteDb>
{
    public void Configure(EntityTypeBuilder<TicketNoteDb> entity)
    {
        #region Reference Properties
        // [Required]
        // [ForeignKey(nameof(Sale))]
        // public int SaleId { get; set; } = default!;
        entity.Property(m => m.TicketId)
			.IsRequired(false);
        #endregion

        #region Navigation Configuration
        // public SaleDb Sale { get; set; } = default!;
        entity.HasOne(m => m.Ticket)
            .WithMany(m => m.Notes)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}