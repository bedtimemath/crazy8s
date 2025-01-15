using System.Text.RegularExpressions;

namespace C8S.Domain;

public static partial class AdminAppRegex
{
    [GeneratedRegex(@".*\((?<code>[A-Z][A-Z])\)")]
    private static partial Regex ParseStateFull();

    public static Match GetStateCodeMatch(string input) => ParseStateFull().Match(input);
}