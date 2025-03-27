using System.Diagnostics;
using C8S.Domain.Enums;
using C8S.Domain.Interfaces;

namespace C8S.Domain.Extensions;

public static class KitEx
{
    public static string ToKitKey(this IKit kit) => String.Join('.',
        (new List<string?>()
        {
            "C8",
            $"S{kit.Season}",
            kit.Year,
            kit.Version,
            kit.AgeLevel switch
            {
                AgeLevel.GradesK2 => "K2",
                AgeLevel.Grades35 => "35",
                _ => throw new UnreachableException($"Unrecognized AgeLevel: {kit.AgeLevel}")
            }
        })
        .Where(s => !String.IsNullOrEmpty(s)));
}