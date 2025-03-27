using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Tickets")]
public class TicketDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => TicketId;
    [NotMapped] 
    public override string Display => SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int TicketId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketStatus Status { get; set; } = default!;
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; }
    public PlaceDb? Place { get; set; } = null!;
    
    [Required, ForeignKey(nameof(Request))]
    public int? RequestId { get; set; }
    public RequestDb? Request { get; set; } = null!;

    [ForeignKey(nameof(Invoice))]
    public int? InvoiceId { get; set; }
    public InvoiceDb? Invoice { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ClubDb> Clubs { get; set; } = null!;
    public ICollection<TicketPersonDb> TicketPersons { get; set; } = null!;
    public ICollection<TicketNoteDb> Notes { get; set; } = null!;
    #endregion
}