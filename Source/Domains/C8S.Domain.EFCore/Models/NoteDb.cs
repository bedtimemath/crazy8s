using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Notes")]
public abstract class NoteDb: BaseCoreDb
{
    #region Override Properties
    [NotMapped]
    public override int Id => NoteId;

    [NotMapped]
    public override string Display => $"{Content} [{Author}]";
    #endregion

    #region Id Property
    [Required]
    public int NoteId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public NoteReference Reference { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string Content { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.FullName)]
    public string Author { get; set; } = default!;
    #endregion
}