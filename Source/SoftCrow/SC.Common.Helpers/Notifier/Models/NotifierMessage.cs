using SC.Common.Helpers.Notifier.Enums;

namespace SC.Common.Helpers.Notifier.Models;

// set up to match NotificationMessage in Radzen
public record NotifierMessage
{
    public NotifierSeverity Level { get; init; } = NotifierSeverity.Info;
    public string? Summary { get; set; }
    public string Detail { get; set; } = null!;
}