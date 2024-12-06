using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("SalePersons")]
public class SalePersonDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => SalePersonId;
    [NotMapped] 
    public override string Display => IsPrimary.ToString();
    #endregion

    #region Id Property
    [Required] 
    public int SalePersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public bool IsPrimary { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; } = default!;
    public PersonDb Person { get; set; } = default!;

    [ForeignKey(nameof(Sale))]
    public int SaleId { get; set; } = default!;
    public SaleDb Sale { get; set; } = default!;
    #endregion
}