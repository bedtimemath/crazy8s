using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using WordPressPCL.Models;

namespace C8S.WordPress.Mapping;

internal class WPUserProfiles : Profile
{
    public WPUserProfiles()
    {
        CreateMap<User, WPUserDetails>()
            .ForMember(m => m.RoleSlugs, opt => opt.MapFrom(m => m.Roles));
        CreateMap<WPUserDetails, User>()
            .ForMember(m => m.Roles, opt => opt.MapFrom(m => m.RoleSlugs));
    }
}