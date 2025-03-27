using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;

namespace C8S.Domain.EFCore.Models;

public class ClubNoteDb: NoteDb
{
    [ForeignKey(nameof(Club))]
    public int? ClubId { get; set; } = null!;
    public ClubDb Club { get; set; } = null!;

    public ClubNoteDb() { Reference = NoteReference.Club; }
}

public class InvoiceNoteDb: NoteDb
{
    [ForeignKey(nameof(Invoice))]
    public int? InvoiceId { get; set; } = null!;
    public InvoiceDb Invoice { get; set; } = null!;

    public InvoiceNoteDb() { Reference = NoteReference.Invoice; }
}

public class OrderNoteDb: NoteDb
{
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; } = null!;
    public OrderDb Order { get; set; } = null!;

    public OrderNoteDb() { Reference = NoteReference.Order; }
}

public class PersonNoteDb: NoteDb
{
    [ForeignKey(nameof(Person))]
    public int? PersonId { get; set; } = null!;
    public PersonDb Person { get; set; } = null!;

    public PersonNoteDb() { Reference = NoteReference.Person; }
}

public class PlaceNoteDb: NoteDb
{
    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; } = null!;
    public PlaceDb Place { get; set; } = null!;

    public PlaceNoteDb() { Reference = NoteReference.Place; }
}

public class TicketNoteDb: NoteDb
{
    [ForeignKey(nameof(Ticket))]
    public int? TicketId { get; set; } = null!;
    public TicketDb Ticket { get; set; } = null!;

    public TicketNoteDb() { Reference = NoteReference.Ticket; }
}