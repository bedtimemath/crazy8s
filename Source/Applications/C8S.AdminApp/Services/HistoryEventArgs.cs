using C8S.Database.Abstractions.Base;

namespace C8S.AdminApp.Services;

public class HistoryEventArgs(
    HistoryAction action,
    BaseDTO target) : EventArgs
{
    public HistoryAction Action { get; set; } = action;
    public BaseDTO Target { get; set; } = target;
}