using Microsoft.AspNetCore.SignalR.Client;
using SC.Common;
using SC.Common.Interfaces;
using SC.Common.PubSub;
using SC.Messaging.Services;

namespace C8S.AdminApp.Client.Services;

public sealed class AdminPubSubService(
    ILoggerFactory loggerFactory): PubSubService(loggerFactory), IAsyncInitializable, IAsyncDisposable
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AdminPubSubService> _logger = loggerFactory.CreateLogger<AdminPubSubService>();
    #endregion

    #region Private Variables
    private HubConnection? _hubConnection = null;
    #endregion

    #region Initialize / Dispose Methods
    public async ValueTask InitializeAsync(IServiceProvider provider)
    {
        _logger.LogDebug("[{Guid:D}] Initializing", UniqueIdentifier);
        if (_hubConnection != null) return;

        try
        {
            var http = provider.GetRequiredService<IHttpClientFactory>();
            var backendClient = http.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var busUrl = backendClient.BaseAddress + "communication";
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

    // see: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync
    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
    }
    #endregion

    #region Event Handlers
    private void HandleDataChangeMessage(DataChange dataChange) =>
        Task.Run(async () => await Publish(dataChange));
    #endregion
}