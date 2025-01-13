using C8S.AdminApp.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Audit.Abstractions.Models;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public sealed partial class Home : BaseRazorPage, IDisposable
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public ICommunicationService CommunicationService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await CommunicationService.InitializeAsync();
        CommunicationService.DataChanged += HandleDataChanged;
        await base.OnInitializedAsync();
    }

    public void Dispose()
    {
        CommunicationService.DataChanged -= HandleDataChanged;
    }

    private void HandleDataChanged(object? sender, DataChangedEventArgs args)
    {
        Logger.LogInformation("HandleDataChanged: {@Args}", args);

        var dataChange = args.DataChange;
        NotificationService.Notify(NotificationSeverity.Info,
            "Data Changed", $"Id: {dataChange.EntityId}; Name: {dataChange.EntityName}; State: {dataChange.EntityState}");
    }
}