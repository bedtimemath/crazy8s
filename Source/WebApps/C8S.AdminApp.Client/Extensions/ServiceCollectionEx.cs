using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Navigation;
using C8S.AdminApp.Client.Services.Pages;
using SC.Common.Client.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddLocalServices(
        this IServiceCollection services)
    {
        services.AddScoped<PagesService>();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IPubSubService, AdminPubSubService>();
    }
}