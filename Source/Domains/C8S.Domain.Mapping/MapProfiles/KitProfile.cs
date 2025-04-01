using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Kits.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class KitProfile: Profile
{
    public KitProfile()
    {
        CreateMap<KitDb, Kit>();
    }
}
