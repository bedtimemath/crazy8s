using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Notes.Models;

namespace C8S.AdminApp.MapProfiles;

internal class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteDb, NoteDetails>();

        CreateMap<ClubNoteDb, NoteDetails>();
        CreateMap<InvoiceNoteDb, NoteDetails>();
        CreateMap<OrderNoteDb, NoteDetails>();
        CreateMap<PersonNoteDb, NoteDetails>();
        CreateMap<PlaceNoteDb, NoteDetails>();
        CreateMap<RequestNoteDb, NoteDetails>();
        CreateMap<SaleNoteDb, NoteDetails>();
    }
}
