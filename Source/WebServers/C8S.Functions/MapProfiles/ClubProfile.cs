using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Functions.DTOs;

namespace C8S.Functions.MapProfiles;

internal class ClubProfile: Profile
{
    public ClubProfile()
    {
        CreateMap<ClubDb, ClubDTO>()
            .ForMember(m => m.ClubStatus, opt => opt.MapFrom(m => m.Status))
            .ForMember(m => m.KitStatus, opt => opt.MapFrom(m => m.Kit.Status))
            .ForMember(m => m.Key, opt => opt.MapFrom(m => m.Kit.Key))
            .ForMember(m => m.Year, opt => opt.MapFrom(m => m.Kit.Year))
            .ForMember(m => m.Season, opt => opt.MapFrom(m => m.Kit.Season))
            .ForMember(m => m.AgeLevel, opt => opt.MapFrom(m => m.Kit.AgeLevel))
            .ForMember(m => m.Version, opt => opt.MapFrom(m => m.Kit.Version));
    }
}