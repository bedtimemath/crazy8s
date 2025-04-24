using System.ComponentModel;
using SC.Common.Attributes;
using SC.Common.Extensions;

namespace SC.Common.Models;

public class EnumDescription<TEnum>(string display, TEnum value)
    where TEnum : struct, Enum
{
    #region Static Methods
    public static IEnumerable<EnumDescription<TEnum>> GetAllEnumDescriptions() =>
        Enum.GetValues<TEnum>().Select(e => new EnumDescription<TEnum>(e));
    public static IEnumerable<EnumDescription<TEnum>> GetAllEnumDescriptions(int min, int max, params TEnum[] extras) =>
        Enum.GetValues<TEnum>()
            .Where(e => (int)Convert.ChangeType(e, typeof(int)) >= min &&
                        (int)Convert.ChangeType(e, typeof(int)) <= max)
            .Union(extras)
            .Select(e => new EnumDescription<TEnum>(e));
    #endregion
    
    #region Public Properties
    public string Display { get; private set; } = display;
    public TEnum Value { get; private set; } = value;
    #endregion

    #region Constructors / Destructor
    public EnumDescription(TEnum value) : 
        this(String.Empty, value)
    {
        var label = value.GetAttributeOfType<LabelAttribute>()?.Label ?? value.ToString();
        var description = value.GetAttributeOfType<DescriptionAttribute>()?.Description ?? null;
        Display = String.IsNullOrEmpty(description) ? label : $"<b>{label}</b>: {description}";
    }

    #endregion
}