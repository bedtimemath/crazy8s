using C8S.Common.Attributes;
using C8S.Common.Extensions;

namespace C8S.Common.Models;

public class EnumLabel<TEnum>(string display, TEnum value)
    where TEnum : struct, Enum
{
    #region Static Methods
    public static IEnumerable<EnumLabel<TEnum>> GetAllEnumLabels() =>
        Enum.GetValues<TEnum>().Select(e => new EnumLabel<TEnum>(e));
    public static IEnumerable<EnumLabel<TEnum>> GetAllEnumLabels(int min, int max, params TEnum[] extras) =>
        Enum.GetValues<TEnum>()
            .Where(e => (int)Convert.ChangeType(e, typeof(int)) >= min &&
                        (int)Convert.ChangeType(e, typeof(int)) <= max)
            .Union(extras)
            .Select(e => new EnumLabel<TEnum>(e));
    #endregion
    
    #region Public Properties
    public string Display { get; private set; } = display;
    public TEnum Value { get; private set; } = value;

    #endregion

    #region Constructors / Destructor
    public EnumLabel(TEnum value) : 
        this(value.GetAttributeOfType<LabelAttribute>()?.Label ?? value.ToString(), value)
    {
    }

    #endregion
}