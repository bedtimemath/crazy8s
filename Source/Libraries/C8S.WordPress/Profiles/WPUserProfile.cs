using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using WordPressPCL.Models;

namespace C8S.WordPress.Profiles;

internal class WPUserProfile : Profile
{
    public WPUserProfile()
    {
        CreateMap<User, WordPressUserDetails>();
        CreateMap<WordPressUserDetails, User>();
    }
}