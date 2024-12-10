using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;

namespace C8S.Domain.EFCore.Models;

public class ClubNoteDb: NoteDb
{
    [ForeignKey(nameof(Club))]
    public int? ClubId { get; set; } = default!;
    public ClubDb Club { get; set; } = default!;

    public ClubNoteDb() { Reference = NoteReference.Club; }
}

public class InvoiceNoteDb: NoteDb
{
    [ForeignKey(nameof(Invoice))]
    public int? InvoiceId { get; set; } = default!;
    public InvoiceDb Invoice { get; set; } = default!;

    public InvoiceNoteDb() { Reference = NoteReference.Invoice; }
}

public class PersonNoteDb: NoteDb
{
    [ForeignKey(nameof(Person))]
    public int? PersonId { get; set; } = default!;
    public PersonDb Person { get; set; } = default!;

    public PersonNoteDb() { Reference = NoteReference.Person; }
}

public class PlaceNoteDb: NoteDb
{
    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; } = default!;
    public PlaceDb Place { get; set; } = default!;

    public PlaceNoteDb() { Reference = NoteReference.Place; }
}

public class RequestNoteDb: NoteDb
{
    [ForeignKey(nameof(Request))]
    public int? RequestId { get; set; } = default!;
    public RequestDb Request { get; set; } = default!;

    public RequestNoteDb() { Reference = NoteReference.Request; }
}

public class SaleNoteDb: NoteDb
{
    [ForeignKey(nameof(Sale))]
    public int? SaleId { get; set; } = default!;
    public SaleDb Sale { get; set; } = default!;

    public SaleNoteDb() { Reference = NoteReference.Sale; }
}

public class OrderNoteDb: NoteDb
{
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; } = default!;
    public OrderDb Order { get; set; } = default!;

    public OrderNoteDb() { Reference = NoteReference.Order; }
}