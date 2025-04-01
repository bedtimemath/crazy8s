using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using SC.Common;

namespace C8S.WordPress.Mapping;

internal class CustomTitleToStringConverter : ITypeConverter<CustomTitle,string>
{
    public string Convert(CustomTitle source, string destination, ResolutionContext context)
        => source?.Rendered ?? SoftCrowConstants.Display.NotSet;
}