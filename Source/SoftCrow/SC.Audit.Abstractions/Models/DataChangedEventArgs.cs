using SC.Common.PubSub;

namespace SC.Audit.Abstractions.Models;

public class DataChangedEventArgs(
    DataChange dataChange): EventArgs
{
    public DataChange DataChange { get; set; } = dataChange;
}