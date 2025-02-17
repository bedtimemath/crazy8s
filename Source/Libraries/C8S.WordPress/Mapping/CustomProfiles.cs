using AutoMapper;
using C8S.Domain.Enums;
using C8S.WordPress.Custom;

namespace C8S.WordPress.Mapping;

internal class CustomProfiles : Profile
{
    public CustomProfiles()
    {
        CreateMap<CustomTitle, string>()
            .ConvertUsing<CustomTitleToStringConverter>();
        CreateMap<string, CustomTitle>()
            .ConvertUsing(m => new CustomTitle() { Rendered = m });

        CreateMap<SkuStatus, string>()
            .ConvertUsing<SkuStatusToStringConverter>();
        CreateMap<string, SkuStatus>()
            .ConvertUsing<StringToSkuStatusConverter>();
    }
}