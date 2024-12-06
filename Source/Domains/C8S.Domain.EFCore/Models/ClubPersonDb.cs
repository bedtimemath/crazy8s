using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("ClubPersons")]
public class ClubPersonDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubPersonId;
    [NotMapped] 
    public override string Display => IsPrimary.ToString();
    #endregion

    #region Id Property
    [Required] 
    public int ClubPersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public bool IsPrimary { get; set; } = default!;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; } = default!;
    public PersonDb Person { get; set; } = default!;

    [ForeignKey(nameof(Club))]
    public int ClubId { get; set; } = default!;
    public ClubDb Club { get; set; } = default!;
    #endregion
}