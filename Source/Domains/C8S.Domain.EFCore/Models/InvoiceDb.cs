using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;
using SC.Common;
using System.Text.Json.Serialization;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Invoices")]
public class InvoiceDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => InvoiceId;
    [NotMapped] 
    public override string Display => InvoiceId.ToString();
    #endregion

    #region Id Property
    [Required] 
    public int InvoiceId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public InvoiceStatus Status { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string Identifier { get; set; } = default!;
    #endregion

    #region Reference Properties
    // one-to-one
    public SaleDb? Sale { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<InvoicePersonDb> InvoicePersons { get; set; } = default!;
    public ICollection<InvoiceNoteDb> Notes { get; set; } = default!;
    #endregion
}