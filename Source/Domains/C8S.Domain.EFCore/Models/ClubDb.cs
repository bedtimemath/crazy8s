using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Clubs")]
public class ClubDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubId;
    [NotMapped] 
    public override string Display => Kit?.KitKey ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int ClubId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubStatus Status { get; set; } = default!;

    public DateOnly? StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Kit))]
    public int KitId { get; set; }
    public KitDb Kit { get; set; } = null!;
    
    [Required, ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }
    public PlaceDb Place { get; set; } = null!;

    [ForeignKey(nameof(Ticket))]
    public int? TicketId { get; set; }
    public TicketDb? Ticket { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ClubPersonDb> ClubPersons { get; set; } = null!;
    public ICollection<OrderDb> Orders { get; set; } = null!;
    public ICollection<ClubNoteDb> Notes { get; set; } = null!;
    #endregion
}