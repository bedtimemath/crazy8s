using SC.Common.Helpers.Notifier.Models;

namespace SC.Common.Helpers.Notifier.Enums;

public class NotifierEventArgs: EventArgs
{
    public NotifierMessage NotifierMessage { get; set; } = null!;
}