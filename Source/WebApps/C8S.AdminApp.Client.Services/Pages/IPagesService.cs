namespace C8S.AdminApp.Client.Services.Pages;

public interface IPagesService
{
    event EventHandler<PageChangedEventArgs>? PageChanged;
}