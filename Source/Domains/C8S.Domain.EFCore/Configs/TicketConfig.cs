using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class TicketConfig : BaseCoreConfig<TicketDb>
{
    public override void Configure(EntityTypeBuilder<TicketDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.TicketId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public SaleStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[Required, ForeignKey(nameof(Place))]
        //public int PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(true);

        //[ForeignKey(nameof(Invoice))]
        //public int? InvoiceId { get; set; } = default!;
        entity.Property(m => m.InvoiceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb? Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Tickets)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);
        
        //public RequestDb? Request { get; set; } = default!;
        entity.HasOne(m => m.Request)
            .WithOne(m => m.Ticket)
            .HasForeignKey<TicketDb>(m => m.RequestId)
            .IsRequired(false);

        //public InvoiceDb? Invoice { get; set; } = default!;
        entity.HasOne(m => m.Invoice)
            .WithOne(m => m.Ticket)
            .HasForeignKey<TicketDb>(m => m.InvoiceId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = default!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Ticket)
            .IsRequired(false);

        //public ICollection<SalePersonDb> SalePersons { get; set; } = default!;
        entity.HasMany(m => m.TicketPersons)
            .WithOne(m => m.Ticket)
            .IsRequired(false);

        //public ICollection<SaleNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Ticket)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}