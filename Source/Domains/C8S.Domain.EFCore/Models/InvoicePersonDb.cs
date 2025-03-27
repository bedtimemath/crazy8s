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
    public override string Display => $"{Invoice}<=>{Person} [{Ordinal}]";
    #endregion

    #region Id Property
    [Required]
    public int InvoicePersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public int Ordinal { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; }
    public PersonDb Person { get; set; } = null!;

    [ForeignKey(nameof(Invoice))]
    public int InvoiceId { get; set; }
    public InvoiceDb Invoice { get; set; } = null!;
    #endregion
}