using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Functions.DTOs;

namespace C8S.Functions.MapProfiles;

internal class ClubProfile: Profile
{
    public ClubProfile()
    {
        CreateMap<ClubDb, ClubDTO>()
            .ReverseMap();
    }
}