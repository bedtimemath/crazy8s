using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Database.Abstractions.Base;
using SC.Common;

namespace C8S.Database.EFCore.Models;

[Table("WorkshopCodes")]
public class WorkshopCodeDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => WorkshopCodeId;
    [NotMapped] 
    public override string Display =>  Key;
    #endregion

    #region Id Property
    [Required] 
    public int WorkshopCodeId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SharedConstants.MaxLengths.Key)]
    public string Key { get; set; } = default!;

    public DateTimeOffset? StartsOn { get; set; } = null;

    public DateTimeOffset? EndsOn { get; set; } = null;
    #endregion
}