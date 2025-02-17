using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using WordPressPCL.Models;

namespace C8S.WordPress.Mapping;

internal class WPUserProfiles : Profile
{
    public WPUserProfiles()
    {
        CreateMap<User, WPUserDetails>();
        CreateMap<WPUserDetails, User>();
    }
}