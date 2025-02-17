using AutoMapper;
using C8S.Domain.Enums;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Custom;

namespace C8S.WordPress.Mapping;

internal class WPSkuProfiles : Profile
{
    public WPSkuProfiles()
    {
        CreateMap<CustomSkuCreate, WPSkuCreate>()
            .ForMember(m => m.Properties, opts => opts.MapFrom(m => m.ACF));
        CreateMap<WPSkuCreate, CustomSkuCreate>()
            .ForMember(m => m.ACF, opts => opts.MapFrom(m => m.Properties));
        
        CreateMap<CustomSku, WPSkuDetails>()
            .ForMember(m => m.Properties, opts => opts.MapFrom(m => m.ACF));
        CreateMap<WPSkuDetails, CustomSku>()
            .ForMember(m => m.ACF, opts => opts.MapFrom(m => m.Properties));

        CreateMap<CustomSkuACF, WPSkuProperties>();
        CreateMap<WPSkuProperties, CustomSkuACF>();

        CreateMap<AgeLevel,string>()
            .ConvertUsing<AgeLevelToStringConverter>();
        CreateMap<string,AgeLevel>()
            .ConvertUsing<StringToAgeLevelConverter>();
    }
}