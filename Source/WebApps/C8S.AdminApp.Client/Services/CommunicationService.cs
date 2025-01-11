using C8S.AdminApp.Common;
using C8S.AdminApp.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Client.Services;

public sealed class CommunicationService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager): IAsyncDisposable, ICommunicationService
{
    private readonly ILogger<CommunicationService> _logger = loggerFactory.CreateLogger<CommunicationService>();

    public event EventHandler<DataChangeEventArgs>? DataChanged;

    private void RaiseDataChanged(DataChange dataChange) =>
        DataChanged?.Invoke(this, new DataChangeEventArgs(dataChange));

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