using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class SaleConfig : BaseCoreConfig<SaleDb>
{
    public override void Configure(EntityTypeBuilder<SaleDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.SaleId);
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

        //[ForeignKey(nameof(Request))]
        //public int? RequestId { get; set; } = default!;
        entity.Property(m => m.RequestId)
            .IsRequired(false);

        //[ForeignKey(nameof(Invoice))]
        //public int? InvoiceId { get; set; } = default!;
        entity.Property(m => m.InvoiceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb? Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Sales)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);

        //public RequestDb? Request { get; set; } = default!;
        entity.HasOne(m => m.Request)
            .WithOne(m => m.Sale)
            .HasForeignKey<SaleDb>(m => m.RequestId)
            .IsRequired(false);

        //public InvoiceDb? Invoice { get; set; } = default!;
        entity.HasOne(m => m.Invoice)
            .WithOne(m => m.Sale)
            .HasForeignKey<SaleDb>(m => m.InvoiceId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = default!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Sale)
            .IsRequired(false);

        //public ICollection<SalePersonDb> SalePersons { get; set; } = default!;
        entity.HasMany(m => m.SalePersons)
            .WithOne(m => m.Sale)
            .IsRequired(false);

        //public ICollection<SaleNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Sale)
            .HasForeignKey(m => m.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}