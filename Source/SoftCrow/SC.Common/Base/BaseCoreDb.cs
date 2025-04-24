using System.ComponentModel.DataAnnotations;

namespace SC.Common.Base;

public abstract class BaseCoreDb: BaseDb, ICoreDb
{
    #region Public Properties
    [Required]
    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
    #endregion
}