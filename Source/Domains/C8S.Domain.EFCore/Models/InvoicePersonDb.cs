using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("InvoicePersons")]
public class InvoicePersonDb : BaseDb
{
    #region Override Properties
    [NotMapped]
    public override int Id => InvoicePersonId;
    [NotMapped]
    public override string Display => IsPrimary.ToString();
    #endregion

    #region Id Property
    [Required]
    public int InvoicePersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public bool IsPrimary { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; } = default!;
    public PersonDb Person { get; set; } = default!;

    [ForeignKey(nameof(Invoice))]
    public int InvoiceId { get; set; } = default!;
    public InvoiceDb Invoice { get; set; } = default!;
    #endregion
}