using C8S.AdminApp.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Common.Services;

public class PubSubService(
    ILoggerFactory loggerFactory) : IPubSubService
{
    protected readonly ILogger<PubSubService> Logger = loggerFactory.CreateLogger<PubSubService>();

    protected readonly Dictionary<Type, List<Delegate>> Handlers = [];

    private bool _hasBeenDisposed = false;

    ~PubSubService()
    {
        Dispose(false);
    }

    public virtual ValueTask InitializeAsync() => default;

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Subscribe<T>(Func<T, Task> handler) where T : class
    {
        if (!Handlers.ContainsKey(typeof(T)))
            Handlers[typeof(T)] = [];

        Handlers[typeof(T)].Add(handler);
    }

    public void Unsubscribe<T>(Func<T, Task> handler) where T : class
    {
        if (!Handlers.ContainsKey(typeof(T))) return;

        Handlers[typeof(T)].Remove(handler);
    }

    public async Task Publish<T>(T notification) where T : class
    {
        if (!Handlers.ContainsKey(typeof(T))) return;

        foreach (var handler in Handlers[typeof(T)].Cast<Func<T, Task>>())
            await handler(notification).ConfigureAwait(false);
    }

    protected virtual void PerformDisposal() {}
    protected virtual ValueTask PerformDisposalAsync() => default;
    
    private void Dispose(bool isDisposing)
    {
        if (_hasBeenDisposed) return;
        if (isDisposing) PerformDisposal();
        _hasBeenDisposed = true;
    }

    private async ValueTask DisposeAsyncCore()
    {
        if (_hasBeenDisposed) return;
        await PerformDisposalAsync();
        _hasBeenDisposed = true;
    }

}