using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("KitPages")]
public class KitPageDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => KitPageId;
    [NotMapped] 
    public override string Display => Title;
    #endregion

    #region Id Property
    [Required] 
    public int KitPageId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KitPageStatus Status { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Url)]
    public string Url { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Title)]
    public string Title { get; set; } = null!;
    #endregion

    #region Child Properties
    public ICollection<KitDb> Kits { get; set; } = null!;
    public ICollection<PermissionDb> Permissions { get; set; } = null!;
    #endregion
}