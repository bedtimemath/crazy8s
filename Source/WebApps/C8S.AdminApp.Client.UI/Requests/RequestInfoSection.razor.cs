using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestInfoSection: BaseClientComponent
{
    [Inject]
    public ILogger<RequestInfoSection> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestDetailsCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.IdSet += HandleIdSet;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Coordinator.IdSet -= HandleIdSet;
    }

    private void HandleIdSet(object? sender, EventArgs e) => 
        StateHasChanged();
}
