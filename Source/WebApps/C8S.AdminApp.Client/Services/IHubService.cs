namespace C8S.AdminApp.Client.Services;

public interface IHubService: IAsyncDisposable
{
    ValueTask InitializeAsync(IServiceProvider provider);
}