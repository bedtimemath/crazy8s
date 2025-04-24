namespace SC.Common.Extensions;

public static class PhoneEx
{
    public static string? DisplayPhone(this string input)
    {
        if (String.IsNullOrWhiteSpace(input)) return null;

        string? area = null, exchange = null, number = null, extra = null;

        var stringMatch = SoftCrowRegex.GetMatchForUSPhoneString(input);
        if (!stringMatch.Success)
        {
            var digitalMatch = SoftCrowRegex.GetMatchForUSPhoneDigital(input);
            if (digitalMatch.Success)
            {
                var digits = digitalMatch.Groups["digits"].Value;
                area = digits[..3];
                exchange = digits[3..^4];
                number = digits[^4..];
                extra = stringMatch.Groups["extra"].Value;
            }
            else
                return input;
        }
        else
        {
            area = stringMatch.Groups["area"].Value;
            exchange = stringMatch.Groups["exchange"].Value;
            number = stringMatch.Groups["number"].Value;
            extra = stringMatch.Groups["extra"].Value;
        }

        var display = $"({area}) {exchange}-{number}";
        if (!String.IsNullOrEmpty(extra)) display += $" {extra}";
        return display;
    }
}