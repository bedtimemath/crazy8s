using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class InvoiceConfig : BaseCoreConfig<InvoiceDb>
{
    public override void Configure(EntityTypeBuilder<InvoiceDb> entity)
    {
        #region Id Property
        // [Required]
        // public int InvoiceId { get; set; }
        entity.HasKey(m => m.InvoiceId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public InvoiceStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string Identifier { get; set; } = default!;
        entity.Property(m => m.Identifier)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Sale))]
        //public int? SaleId { get; set; }
        entity.Property(m => m.SaleId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public SaleDb? Sale { get; set; }
        entity.HasOne(m => m.Sale)
            .WithOne(m => m.Invoice)
            .HasForeignKey<InvoiceDb>(m => m.SaleId)
            .IsRequired(false);

        //public ICollection<InvoicePersonDb> InvoicePersons { get; set; } = default!;
        entity.HasMany(m => m.InvoicePersons)
            .WithOne(m => m.Invoice)
            .HasForeignKey(m => m.InvoiceId)
            .IsRequired(false);

        //public ICollection<InvoiceNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Invoice)
            .HasForeignKey(m => m.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}