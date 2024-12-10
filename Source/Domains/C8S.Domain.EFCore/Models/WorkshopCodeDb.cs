using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

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
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Key)]
    public string Key { get; set; } = default!;

    public DateTimeOffset? StartsOn { get; set; }

    public DateTimeOffset? EndsOn { get; set; }
    #endregion
}