using Microsoft.Extensions.Logging;
using SC.Common.Helpers.Notifier.Enums;
using SC.Common.Helpers.Notifier.Models;

namespace SC.Common.Helpers.PubSub.Services;

public sealed class PubSubService(
    ILoggerFactory loggerFactory) : IPubSubService
{
    #region ReadOnly Constructor Variables
    //private readonly ILogger<PubSubService> _logger = loggerFactory.CreateLogger<PubSubService>();
    #endregion

    #region Protected Variables
    private readonly Lock _handlersLock = new();
    private readonly Dictionary<Type, List<Delegate>> _handlers = []; 
    #endregion

    #region Public Methods
    public void Subscribe<T>(Func<T, Task> handler) where T : class
    {
        lock (_handlersLock)
        {
            if (!_handlers.ContainsKey(typeof(T)))
                _handlers[typeof(T)] = [];
            _handlers[typeof(T)].Add(handler);
        }
    }

    public void Unsubscribe<T>(Func<T, Task> handler) where T : class
    {
        lock (_handlersLock)
        {
            if (!_handlers.ContainsKey(typeof(T))) return;
            _handlers[typeof(T)].Remove(handler);
        }
    }

    public void Publish<T>(T notification) where T : class
    {
        lock (_handlersLock)
        {
            if (!_handlers.ContainsKey(typeof(T))) return;

            //_logger.LogInformation("Publish:{EventType}:{Payload}:{Count:#,##0} subscribers",  
            //    typeof(T), notification, _handlers[typeof(T)].Count);

            foreach (var handler in _handlers[typeof(T)].Cast<Func<T, Task>>())
                Task.Run(async () => await handler(notification).ConfigureAwait(false));
        }
    }

    public void PublishException(Exception exception) =>
        Publish(new NotifierMessage()
            {
                Level = NotifierSeverity.Error,
                Summary = exception.Message,
                Detail = exception.StackTrace ?? string.Empty
        });
    #endregion
}