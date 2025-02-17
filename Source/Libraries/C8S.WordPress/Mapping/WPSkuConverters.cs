using AutoMapper;
using C8S.Domain.Enums;

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

internal class SkuStatusToStringConverter : ITypeConverter<SkuStatus, string>
{
    public string Convert(SkuStatus source, string destination, ResolutionContext context)
        => source switch
        {
            SkuStatus.Active => "publish",
            SkuStatus.Draft => "draft",
            SkuStatus.Inactive => "trash",
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}

internal class StringToSkuStatusConverter : ITypeConverter<string, SkuStatus>
{
    public SkuStatus Convert(string  source, SkuStatus destination, ResolutionContext context)
        => source switch
        {
            "publish" => SkuStatus.Active,
            "draft" => SkuStatus.Draft,
            "trash" => SkuStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}