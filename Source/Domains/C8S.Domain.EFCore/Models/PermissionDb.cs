using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Permissions")]
public class PermissionDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => PermissionId;
    [NotMapped] 
    public override string Display => $"{Person}<=>{KitPage}";
    #endregion

    #region Id Property
    [Required] 
    public int PermissionId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public DateTimeOffset? ExpiresOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; }
    public PersonDb Person { get; set; } = null!;

    [ForeignKey(nameof(KitPage))]
    public int KitPageId { get; set; }
    public KitPageDb KitPage { get; set; } = null!;
    #endregion
}