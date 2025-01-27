using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestClubsSection: BaseRazorComponent
{
    [Inject]
    public ILogger<RequestClubsSection> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestDetailsCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.DetailsLoaded += HandleDetailsLoaded;
        base.OnInitialized();
    }
    public void Dispose()
    {
        Coordinator.DetailsLoaded -= HandleDetailsLoaded;
    }

    private void HandleDetailsLoaded(object? sender, EventArgs e) => 
        StateHasChanged();
}
