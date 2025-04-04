using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Menu.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddLocalServices(
        this IServiceCollection services)
    {
        // Initializable
        services.AddSingleton<IHubService, HubService>();
        services.AddSingleton<ISidebarMenuService, SidebarMenuService>();
    }
}