using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.UI.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsList: BaseClientComponent, IDisposable
{    
    [Inject]
    public ILogger<RequestsList> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListController Controller { get; set; } = null!;

    protected override void OnInitialized()
    {
        Controller.SearchClicked += HandleSearchClicked;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Controller.SearchClicked -= HandleSearchClicked;
    }

    private void HandleSearchClicked(object? sender, EventArgs e)
    {
        Logger.LogInformation("Search Clicked");
        StateHasChanged();
    }
}