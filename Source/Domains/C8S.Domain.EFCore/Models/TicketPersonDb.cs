using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("TicketPersons")]
public class TicketPersonDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => TicketPersonId;
    [NotMapped] 
    public override string Display => $"{Ticket}<=>{Person}";
    #endregion

    #region Id Property
    [Required] 
    public int TicketPersonId { get; set; }
    #endregion

    #region Database Properties
    [Required]
    public int Ordinal { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Person))]
    public int PersonId { get; set; }
    public PersonDb Person { get; set; } = null!;

    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    public TicketDb Ticket { get; set; } = null!;
    #endregion
}