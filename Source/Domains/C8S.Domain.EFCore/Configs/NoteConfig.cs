using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class NoteConfig : IEntityTypeConfiguration<NoteDb>
{
    public void Configure(EntityTypeBuilder<NoteDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ItemId { get; set; }
        entity.HasKey(m => m.NoteId);
        #endregion

        #region Inheritance Configuration
        entity.HasDiscriminator(m => m.Reference)
            .HasValue<ClubNoteDb>(NoteReference.Club)
            .HasValue<InvoiceNoteDb>(NoteReference.Invoice)
            .HasValue<PersonNoteDb>(NoteReference.Person)
            .HasValue<PlaceNoteDb>(NoteReference.Place)
            .HasValue<RequestNoteDb>(NoteReference.Request)
            .HasValue<SaleNoteDb>(NoteReference.Sale)
            .HasValue<OrderNoteDb>(NoteReference.Order);
        #endregion

        #region Database Properties
        // [Required]
        // public NoteReference Reference { get; set; }
        entity.Property(m => m.Reference)
            .HasConversion<string>()
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(true);

        // [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
        // public string Content { get; set; } = null;
        entity.Property(m => m.Content)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(true);
        
        // [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
        // public string Author { get; set; }
        entity.Property(m => m.Author)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(true);
        #endregion
    }
}