using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("OldNews")]
public class OldNewDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OldNewId;
    [NotMapped] 
    public override string Display => $"{OldTableName} [{OldId}] <=> {NewTableName} [{NewId}]";
    #endregion

    #region Id Property
    [Required] 
    public int OldNewId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string OldTableName { get; set; } = null!;

    [Required]
    public Guid OldId { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string NewTableName { get; set; } = null!;

    [Required]
    public int NewId { get; set; }
    #endregion
}