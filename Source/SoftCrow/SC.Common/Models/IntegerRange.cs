using System.Text.RegularExpressions;

namespace SC.Common.Models;

public class IntegerRange
{
    #region Constants & ReadOnlys
    private static readonly Regex RegexMinOnly = new Regex(@"(?<Min>\d+)\+", RegexOptions.Compiled);
    private static readonly Regex RegexMaxOnly = new Regex(@"\<(?<Max>\d+)", RegexOptions.Compiled);
    private static readonly Regex RegexRange = new Regex(@"(?<Min>\d+)\-(?<Max>\d+)", RegexOptions.Compiled);
    private static readonly Regex RegexSingle = new Regex(@"(?<Both>\d+)", RegexOptions.Compiled);
    #endregion

    #region Static Properties / Methods
    public static IntegerRange Empty = new IntegerRange();
    public static IntegerRange Parse(string? value)
    {
        if (String.IsNullOrEmpty(value)) return Empty;

        var minOnly = RegexMinOnly.Match(value);
        if (minOnly.Success)
        {
            var min = Int32.Parse(minOnly.Groups["Min"].Value);
            return new IntegerRange() { Min = min };
        }

        var maxOnly = RegexMaxOnly.Match(value);
        if (maxOnly.Success)
        {
            var max = Int32.Parse(maxOnly.Groups["Max"].Value);
            return new IntegerRange() { Max = max };
        }

        var range = RegexRange.Match(value);
        if (range.Success)
        {
            var min = Int32.Parse(range.Groups["Min"].Value);
            var max = Int32.Parse(range.Groups["Max"].Value);
            return new IntegerRange(min, max);
        }

        var single = RegexSingle.Match(value);
        if (single.Success)
        {
            var min = Int32.Parse(single.Groups["Both"].Value);
            var max = Int32.Parse(single.Groups["Both"].Value);
            return new IntegerRange(min, max);
        }

        return Empty;
    }
    #endregion

    #region Public Properties
    public int? Min { get; set; }
    public int? Max { get; set; }
    #endregion

    #region Constructors / Destructor
    public IntegerRange() {}

    public IntegerRange(int? min, int? max)
    {
        Min = min; 
        Max = max;
    }
    #endregion

    #region Public Overrides
    public override string? ToString() => (Min == null && Max == null) ? null :
        ((Min != null && Max != null) ? (Min == Max ? $"{Min}" : $"{Min}-{Max}") : 
            ((Min == null) ? $"<{Max}" : $"{Min}+"));
    #endregion
}