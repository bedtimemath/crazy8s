using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Requests.Models;

namespace C8S.AdminApp.MapProfiles;

internal class RequestProfile: Profile
{
    public RequestProfile()
    {
        CreateMap<RequestDb, RequestAbstract>()
            .Include<RequestDb, RequestDetails>()
            .Include<RequestDb, RequestListItem>()
            .ForMember(dest => dest.StartDates, 
                opt => opt.MapFrom(src => src.RequestedClubs.Select(c => c.StartsOn)));
        
        CreateMap<RequestDb, RequestDetails>();
        CreateMap<RequestDb, RequestListItem>();
    }
}
