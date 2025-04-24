using SC.Common.Helpers.Notifier.Enums;
using SC.Common.Helpers.Notifier.Models;
using SC.Common.Helpers.PubSub.Services;

namespace SC.Common.Helpers.Notifier.Services;

public sealed class NotifierService : INotifierService
{
    private readonly IPubSubService _pubSubService;

    public event EventHandler<NotifierEventArgs>? NotificationReceived;
    private void RaiseNotificationReceived(NotifierMessage notifierMessage) =>
        NotificationReceived?.Invoke(this, new NotifierEventArgs() { NotifierMessage = notifierMessage });

    public NotifierService(
        IPubSubService pubSubService)
    {
        _pubSubService = pubSubService;
        _pubSubService.Subscribe<NotifierMessage>(HandleNotificationReceived);
    }

    ~NotifierService()
    {
        ReleaseUnmanagedResources();
    }
    
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
        _pubSubService.Unsubscribe<NotifierMessage>(HandleNotificationReceived);
    }

    private async Task HandleNotificationReceived(NotifierMessage notifierMessage) =>
        await Task.Run(() => RaiseNotificationReceived(notifierMessage));

}