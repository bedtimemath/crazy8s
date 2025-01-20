using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestCallSection: BaseClientComponent
{
    [Inject]
    public ILogger<RequestCallSection> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestDetailsCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.DetailsUpdated += HandleDetailsUpdated;
        base.OnInitialized();
    }

    private void LogPhone(string input, bool good) =>
        Logger.LogDebug("{Result}: {Input} => {Number}", good ? "GOOD" : "BAD", input,  input.DisplayPhone());

    public void Dispose()
    {
        Coordinator.DetailsUpdated -= HandleDetailsUpdated;
    }

    private void HandleDetailsUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
}
