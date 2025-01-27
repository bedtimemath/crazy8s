using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Models;
using SC.Common;
using SC.Common.Helpers.Interfaces;

namespace C8S.AdminApp.Client.Services.PubSubs;

public class PubSubService(
    ILoggerFactory loggerFactory,
    string busUrl): IPubSubService
{
    private readonly ILogger<PubSubService> _logger = loggerFactory.CreateLogger<PubSubService>();
    private HubConnection? _hubConnection = null;
    
    protected readonly Dictionary<Type, List<Delegate>> Handlers = [];

    private bool _hasBeenDisposed = false;

    ~PubSubService()
    {
        Dispose(false);
    }

    public async ValueTask InitializeAsync()
    {
        if (_hubConnection != null) return;

        try
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(busUrl)
                .Build();
            _hubConnection.On<DataChange>(SoftCrowConstants.Messages.DataChange, HandleDataChangeMessage);
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not initialize communication hub.");
        }
    }

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

    protected virtual async ValueTask PerformDisposalAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
    }
    
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

    private void HandleDataChangeMessage(DataChange dataChange) =>
        Task.Run(async () => await Publish(dataChange));
}