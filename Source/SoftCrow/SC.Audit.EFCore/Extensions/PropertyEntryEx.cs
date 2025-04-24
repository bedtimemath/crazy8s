using Microsoft.EntityFrameworkCore.ChangeTracking;
using SC.Common.Models;

namespace SC.Audit.EFCore.Extensions;

public static class PropertyEntryEx
{
    public static PropertyChangeData ToChangeData(this PropertyEntry entry, bool includeOriginal = false)
    {
        // if it's nullable, we want to use the underlying type
        var clrType = entry.Metadata.ClrType;
        if (entry.Metadata.IsNullable && clrType.IsGenericType && clrType.GenericTypeArguments.Any())
            clrType = clrType.GenericTypeArguments.First();

        // better to have the enum values as strings; easier to read
        var value = (clrType.IsEnum) ? entry.CurrentValue?.ToString() : entry.CurrentValue;
        var originalValue = (clrType.IsEnum) ? entry.OriginalValue?.ToString() : entry.OriginalValue;

        // always add name & value; original only needed for modifieds
        var changeData = new PropertyChangeData()
        {
            Name = entry.Metadata.Name,
            Value = value
        };
        if (includeOriginal)
            changeData.OriginalValue = originalValue;

        return changeData;
    }
}