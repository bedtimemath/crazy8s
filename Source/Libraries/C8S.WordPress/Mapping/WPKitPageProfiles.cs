using AutoMapper;
using C8S.Domain.Enums;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Custom;

namespace C8S.WordPress.Mapping;

internal class WPKitPageProfiles : Profile
{
    public WPKitPageProfiles()
    {
        CreateMap<CustomPostCreate, WPKitPageCreate>()
            .ForMember(m => m.Properties, opts => opts.MapFrom(m => m.ACF));
        CreateMap<WPKitPageCreate, CustomPostCreate>()
            .ForMember(m => m.ACF, opts => opts.MapFrom(m => m.Properties));

        CreateMap<CustomPost, WPKitPageDetails>();
        //    .ForMember(m => m.Properties, opts => opts.MapFrom(m => m.ACF));
        CreateMap<WPKitPageDetails, CustomPost>();
        //    .ForMember(m => m.ACF, opts => opts.MapFrom(m => m.Properties));

        CreateMap<CustomPostACF, WPKitPageProperties>();
        CreateMap<WPKitPageProperties, CustomPostACF>();

        CreateMap<AgeLevel,string>()
            .ConvertUsing<AgeLevelToStringConverter>();
        CreateMap<string,AgeLevel>()
            .ConvertUsing<StringToAgeLevelConverter>();
    }
}