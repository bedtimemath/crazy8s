using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFooter: BaseClientComponent
{
    
    [Inject]
    public ILogger<RequestsFooter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListController Controller { get; set; } = null!;

    protected override void OnInitialized()
    {
        Controller.ListUpdated += HandleListUpdated;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Controller.ListUpdated -= HandleListUpdated;
    }

    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
}
