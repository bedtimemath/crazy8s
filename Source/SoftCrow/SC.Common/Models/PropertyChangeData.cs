namespace SC.Common.Models;

public class PropertyChangeData
{
    public string Name { get; set; } = default!;
    public object? Value { get; set; } = default!;
    public object? OriginalValue { get; set; }
}