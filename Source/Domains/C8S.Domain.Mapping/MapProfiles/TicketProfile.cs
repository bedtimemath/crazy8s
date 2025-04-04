using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Tickets.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class TicketProfile: Profile
{
    public TicketProfile()
    {
        CreateMap<TicketDb, TicketListItem>();
    }
}