using AutoMapper;
using C8S.WordPress.Abstractions.Models;
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

        CreateMap<WPPageStatus, string>()
            .ConvertUsing<WPPageStatusToStringConverter>();
        CreateMap<string, WPPageStatus>()
            .ConvertUsing<StringToWPPageStatusConverter>();
    }
}