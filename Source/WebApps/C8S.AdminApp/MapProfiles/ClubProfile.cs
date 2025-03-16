using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Clubs.Models;

namespace C8S.AdminApp.MapProfiles;

internal class ClubProfile: Profile
{
    public ClubProfile()
    {
        CreateMap<ClubDb, Club>();
        CreateMap<ClubDb, ClubWithOrders>();
    }
}
