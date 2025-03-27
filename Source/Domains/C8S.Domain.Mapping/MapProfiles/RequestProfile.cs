using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Requests.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class RequestProfile: Profile
{
    public RequestProfile()
    {
        CreateMap<RequestDb, RequestAbstract>()
            .Include<RequestDb, RequestDetails>()
            .Include<RequestDb, RequestListItem>();
        
        CreateMap<RequestDb, RequestDetails>();
        CreateMap<RequestDb, RequestListItem>();
    }
}
