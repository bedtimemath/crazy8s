namespace C8S.Common.Extensions;

public static class DateOnlyEx
{
    public static string ToOptionalYearString(this DateOnly date,
        string noYearFormat = "MMM d", string withYearFormat = "MMM d, yyyy")
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var difference = Math.Abs(today.DayNumber - date.DayNumber);
        return difference > 180 ? date.ToString(withYearFormat) : date.ToString(noYearFormat);
    }

    public static string ToOptionalYearString(this DateTime date,
        string noYearFormat = "MMM d", string withYearFormat = "MMM d, yyyy") =>
        DateOnly.FromDateTime(date).ToOptionalYearString(noYearFormat, withYearFormat);
}