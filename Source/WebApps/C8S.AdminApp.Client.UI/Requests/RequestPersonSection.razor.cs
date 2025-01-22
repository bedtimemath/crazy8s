using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestPersonSection: BaseClientComponent
{
    [Inject]
    public ILogger<RequestPersonSection> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestDetailsCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.DetailsUpdated += HandleDetailsUpdated;
        base.OnInitialized();
    }
    public void Dispose()
    {
        Coordinator.DetailsUpdated -= HandleDetailsUpdated;
    }

    private void HandleDetailsUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
}
