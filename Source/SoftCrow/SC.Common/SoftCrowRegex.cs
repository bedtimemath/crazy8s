using System.Text.RegularExpressions;

namespace SC.Common;

// see: https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-source-generators
public static partial class SoftCrowRegex
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex ParseSpaces();
    public static string ReplaceSpaces(string input) => ParseSpaces().Replace(input, " ");
    
    [GeneratedRegex(@"[^\w]+")]
    private static partial Regex ParseNonAlphanumeric();
    public static string ReplaceNonAlphanumeric(string input, string replacement) => ParseNonAlphanumeric().Replace(input, replacement);
    
    [GeneratedRegex(@"([A-Z])")]
    private static partial Regex ParseCapitals();
    public static string ReplaceCapitals(string input) => ParseCapitals().Replace(input, " $1");
 
    [GeneratedRegex(@"[_A-Za-z0-9\-]+")]
    private static partial Regex ParseNonSlug();
    public static string ConvertToSlug(string input) => ParseNonSlug().Replace(input.ToLower(), "_");

    [GeneratedRegex(@"\<i class=""fa\-\w+ fa\-(?<name>[\w\-]+)""\>\</i\>")]
    private static partial Regex ParseFontAwesomeIconString();
    public static string? ReadNameFromFontAwesomeIcon(string input) =>
        ParseFontAwesomeIconString().Match(input)?.Groups["name"]?.Value;

    [GeneratedRegex(@"^\s*\((?<area>\d{3})\)\s*(?<exchange>\d{3})\s*\-?\s*(?<number>\d{4})\s*(?<extra>.*)\s*$")]
    private static partial Regex ParseUSPhoneString();
    public static Match GetMatchForUSPhoneString(string input) => ParseUSPhoneString().Match(input);

    [GeneratedRegex(@"^\s*(?:\+1)?\s*(?<digits>\d{10})(?<extra>.*)?$")]
    private static partial Regex ParseUSPhoneDigital();
    public static Match GetMatchForUSPhoneDigital(string input) => ParseUSPhoneDigital().Match(input);

    [GeneratedRegex(@"^(?<start>\S+?)(?<digits>\d+)$")]
    private static partial Regex ParseEndingDigits();
    public static Match GetMatchForEndingDigits(string input) => ParseEndingDigits().Match(input);
}