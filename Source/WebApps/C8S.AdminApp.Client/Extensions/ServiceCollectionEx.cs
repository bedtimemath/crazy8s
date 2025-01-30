using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Navigation;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddLocalServices(
        this IServiceCollection services)
    {
        services.AddSingleton<ICQRSService, CQRSService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IPubSubService, PubSubService>();

        services.AddSingleton<AppointmentCallbacks>();
        services.AddSingleton<NoteCallbacks>();
        services.AddSingleton<RequestCallbacks>();
    }
}