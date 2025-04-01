using AutoMapper;
using C8S.Domain.Enums;

namespace C8S.WordPress.Mapping;

internal class WPKitPageProfiles : Profile
{
    public WPKitPageProfiles()
    {
        CreateMap<AgeLevel,string>()
            .ConvertUsing<AgeLevelToStringConverter>();
        CreateMap<string,AgeLevel>()
            .ConvertUsing<StringToAgeLevelConverter>();
    }
}