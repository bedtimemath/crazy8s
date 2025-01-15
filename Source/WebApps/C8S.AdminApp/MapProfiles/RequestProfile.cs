using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Requests.Models;

namespace C8S.AdminApp.MapProfiles;

internal class RequestProfile: Profile
{
    public RequestProfile()
    {
        CreateMap<RequestDb, RequestDetails>();
        CreateMap<RequestDb, RequestListItem>();
    }
}