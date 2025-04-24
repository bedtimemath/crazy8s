using System.ComponentModel;
using SC.Common.Attributes;

namespace SC.Common.Extensions;

// see: https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
public static class EnumEx
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? (T)attributes[0] : null;
    }

    public static string? GetLabel(this Enum enumVal) =>
        enumVal.GetAttributeOfType<LabelAttribute>()?.Label ?? enumVal.ToString();

    public static string? GetDescription(this Enum enumVal) =>
        enumVal.GetAttributeOfType<DescriptionAttribute>()?.Description ?? enumVal.ToString();

}