using C8S.AdminApp.Common;
using C8S.AdminApp.Common.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Client.Services.PubSubs;

public sealed class ClientPubSubService(
    ILoggerFactory loggerFactory,
    string busUrl): PubSubService(loggerFactory)
{
    private HubConnection? _hubConnection = null;

    public override async ValueTask InitializeAsync()
    {
        if (_hubConnection != null) return;

        try
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(busUrl)
                .Build();
            _hubConnection.On<DataChange>(AdminAppConstants.Messages.DataChange, HandleDataChangeMessage);
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Could not initialize communication hub.");
        }
    }

    protected override async ValueTask PerformDisposalAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
        await base.PerformDisposalAsync();
    }

    private void HandleDataChangeMessage(DataChange dataChange) =>
        Task.Run(async () => await Publish(dataChange));
}