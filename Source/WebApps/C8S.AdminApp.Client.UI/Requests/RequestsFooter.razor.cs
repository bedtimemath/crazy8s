using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFooter: BaseRazorComponent
{
    
    [Inject]
    public ILogger<RequestsFooter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.ListUpdated += HandleListUpdated;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Coordinator.ListUpdated -= HandleListUpdated;
    }

    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
}
