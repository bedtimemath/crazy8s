using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.ClubPersons.Models;

namespace C8S.AdminApp.MapProfiles;

internal class ClubPersonProfile: Profile
{
    public ClubPersonProfile()
    {
        CreateMap<ClubPersonDb, ClubPerson>();
        CreateMap<ClubPersonDb, ClubPersonWithOrders>();
    }
}
