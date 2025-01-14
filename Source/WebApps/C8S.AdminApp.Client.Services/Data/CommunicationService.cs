using C8S.AdminApp.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Client.Services.Data;

public sealed class CommunicationService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager): IAsyncDisposable, ICommunicationService
{
    private readonly ILogger<CommunicationService> _logger = loggerFactory.CreateLogger<CommunicationService>();

    public event EventHandler<DataChangedEventArgs>? DataChanged;

    private void RaiseDataChanged(DataChange dataChange) =>
        DataChanged?.Invoke(this, new DataChangedEventArgs(dataChange));

    private HubConnection? _hubConnection = null;

    public async Task InitializeAsync()
    {
        if (_hubConnection != null) return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/communication"))
            .Build();
        _hubConnection.On<DataChange>(AdminAppConstants.Messages.DataChange, HandleDataChangeMessage);
        await _hubConnection.StartAsync();
    }

    private void HandleDataChangeMessage(DataChange dataChange)
    {
        _logger.LogInformation("HandleDataChangeMessage: {@Message}", dataChange);
        RaiseDataChanged(dataChange);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
    }
}