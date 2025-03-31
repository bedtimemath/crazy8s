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
    public override string Display => $"{Person}<=>{Club} [{Ordinal}]";
    #endregion

    #region Id Property
    [Required] 
    public int ClubPersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public int Ordinal { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; }
    public PersonDb Person { get; set; } = null!;

    [ForeignKey(nameof(Club))]
    public int ClubId { get; set; }
    public ClubDb Club { get; set; } = null!;
    #endregion
}