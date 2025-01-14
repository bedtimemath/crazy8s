namespace C8S.AdminApp.Client.Services.Controllers.Requests;

public sealed class RequestsListController
{
    public event EventHandler? SearchClicked; 
    public void RaiseSearchClicked() => SearchClicked?.Invoke(this, EventArgs.Empty);
    
    public string? Query { get; set; }
}