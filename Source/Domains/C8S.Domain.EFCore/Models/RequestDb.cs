using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Requests")]
public class RequestDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => RequestId;
    [NotMapped] 
    public override string Display => Ticket?.ToString() ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int RequestId { get; set; }
    #endregion

    #region Database Properties
    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? WorkshopCode { get; set; }

    public long? AppointmentId { get; set; }

    public DateTimeOffset? AppointmentStartsOn { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? ReferenceSource { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Long)]
    public string? ReferenceSourceOther { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Comments)]
    public string? Comments { get; set; }

    [Required]
    public DateTimeOffset SubmittedOn { get; set; } = default!;
    #endregion

    #region Reference Properties
    public TicketDb? Ticket { get; set; }
    #endregion
}