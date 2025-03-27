using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Notes.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteDb, NoteDetails>();

        CreateMap<ClubNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.ClubId));
        CreateMap<InvoiceNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.InvoiceId));
        CreateMap<OrderNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.OrderId));
        CreateMap<PersonNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.PersonId));
        CreateMap<PlaceNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.PlaceId));
        CreateMap<TicketNoteDb, NoteDetails>()
            .ForMember(m => m.ParentId, 
                opt => opt.MapFrom(n => n.TicketId));
    }
}
