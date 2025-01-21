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

public class RequestNoteDb: NoteDb
{
    [ForeignKey(nameof(Request))]
    public int? RequestId { get; set; } = null!;
    public RequestDb Request { get; set; } = null!;

    public RequestNoteDb() { Reference = NoteReference.Request; }
}

public class SaleNoteDb: NoteDb
{
    [ForeignKey(nameof(Sale))]
    public int? SaleId { get; set; } = null!;
    public SaleDb Sale { get; set; } = null!;

    public SaleNoteDb() { Reference = NoteReference.Sale; }
}

public class OrderNoteDb: NoteDb
{
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; } = null!;
    public OrderDb Order { get; set; } = null!;

    public OrderNoteDb() { Reference = NoteReference.Order; }
}