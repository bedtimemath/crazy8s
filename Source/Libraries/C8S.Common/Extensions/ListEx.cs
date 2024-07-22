namespace C8S.Common.Extensions;

public static class ListEx
{
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-add-custom-methods-for-linq-queries
    public static decimal? Median(this IEnumerable<decimal>? source)
    {
        if (source == null) return null;

        var sortedList = source.OrderBy(number => number).ToList();
        if (!sortedList.Any()) return null;

        int itemIndex = sortedList.Count / 2;

        return sortedList.Count % 2 == 0 ?
             // Even number of items.
             (sortedList[itemIndex] + sortedList[itemIndex - 1]) / 2 :
            // Odd number of items.
            sortedList[itemIndex];
    }

    // see: https://stackoverflow.com/questions/3141692/standard-deviation-of-generic-list
    public static double StdDev(this ICollection<double> source, double? average = null)
    {
        var avg = average ?? source.Average();
        return Math.Sqrt(source.Average(v => (v - avg) * (v - avg)));
    }
    public static decimal StdDev(this ICollection<decimal> source, decimal? average = null)
    {
        var avg = average ?? source.Average();
        return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(source.Average(v => (v - avg) * (v - avg)))));
    }
}