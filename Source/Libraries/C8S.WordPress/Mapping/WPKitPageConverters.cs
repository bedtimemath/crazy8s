using AutoMapper;
using C8S.Domain.Enums;
using C8S.WordPress.Abstractions.Models;

namespace C8S.WordPress.Mapping;

internal class AgeLevelToStringConverter : ITypeConverter<AgeLevel, string>
{
    public string Convert(AgeLevel source, string destination, ResolutionContext context)
        => source switch
        {
            AgeLevel.GradesK2 => "K-2nd",
            AgeLevel.Grades35 => "3rd-5th",
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}

internal class StringToAgeLevelConverter : ITypeConverter<string, AgeLevel>
{
    public AgeLevel Convert(string  source, AgeLevel destination, ResolutionContext context)
        => source switch
        {
            "K-2nd" => AgeLevel.GradesK2,
            "3rd-5th" => AgeLevel.Grades35,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}

internal class WPPageStatusToStringConverter : ITypeConverter<WPPageStatus, string>
{
    public string Convert(WPPageStatus source, string destination, ResolutionContext context)
        => source switch
        {
            WPPageStatus.Active => "publish",
            WPPageStatus.Draft => "draft",
            WPPageStatus.Inactive => "trash",
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}

internal class StringToWPPageStatusConverter : ITypeConverter<string, WPPageStatus>
{
    public WPPageStatus Convert(string  source, WPPageStatus destination, ResolutionContext context)
        => source switch
        {
            "publish" => WPPageStatus.Active,
            "draft" => WPPageStatus.Draft,
            "trash" => WPPageStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}