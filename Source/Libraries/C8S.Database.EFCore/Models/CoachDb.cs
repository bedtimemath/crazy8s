#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Coaches")]
public class CoachDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => CoachId;
    [NotMapped] 
    public override string Display => Name + (!String.IsNullOrEmpty(Email) ? $" <{Email}>" : null);
    #endregion

    #region Id Property
    [Required] 
    public int CoachId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SharedConstants.MaxLengths.FullName)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Email)]
    public string Email { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Phone)]
    public string Phone { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CoachStatus Status { get; set; } = CoachStatus.Pending;

    [Required, MaxLength(SharedConstants.MaxLengths.Url)]
    public string Image { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Long)]
    public string? TagLine { get; set; } = null;

    public string? Bio { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Standard)]
    public string? AuthId { get; set; } = null;
    #endregion

    #region Reference Properties
    //[ForeignKey(nameof(Group))]
    //public int? GroupId { get; set; } = default!;
    //public GroupDb? Group { get; set; } = default!;
    #endregion

    #region Reference Collections
    // one-to-one
    // one-to-many
    //public ICollection<LeadDb> Leads { get; set; } = default!;
    //public ICollection<ActionDb> Actions { get; set; } = default!;

    // many-to-many
    #endregion
}