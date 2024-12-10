using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Permissions")]
public class PermissionDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => PermissionId;
    [NotMapped] 
    public override string Display => IsPrimary.ToString();
    #endregion

    #region Id Property
    [Required] 
    public int PermissionId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public bool IsPrimary { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; } = default!;
    public PersonDb Person { get; set; } = default!;

    [ForeignKey(nameof(Sku))]
    public int SkuId { get; set; } = default!;
    public SkuDb Sku { get; set; } = default!;
    #endregion
}