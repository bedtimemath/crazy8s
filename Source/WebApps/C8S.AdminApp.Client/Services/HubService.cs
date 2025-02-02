using Microsoft.AspNetCore.SignalR.Client;
using SC.Common.PubSub;
using SC.Common;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services;

public sealed class HubService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService) : IHubService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<HubService> _logger = loggerFactory.CreateLogger<HubService>();
    #endregion

    #region Private Variables
    private HubConnection? _hubConnection = null;
    #endregion

    #region Initialize / Dispose Methods
    public async ValueTask InitializeAsync(IServiceProvider provider)
    {
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
        Task.Run(async () => await pubSubService.Publish(dataChange));
    #endregion

}