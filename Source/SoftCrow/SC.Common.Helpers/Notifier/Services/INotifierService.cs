using SC.Common.Helpers.Notifier.Enums;

namespace SC.Common.Helpers.Notifier.Services;

public interface INotifierService: IDisposable
{
    event EventHandler<NotifierEventArgs> NotificationReceived;
}