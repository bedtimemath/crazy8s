using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Menu.Services;
using C8S.AdminApp.Client.Services.Navigation.Services;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddLocalServices(
        this IServiceCollection services)
    {
        // SoftCrow Standard
        services.AddSingleton<ICQRSService, CQRSService>();
        services.AddSingleton<IPubSubService, PubSubService>();
        services.AddSingleton<INotifierService, NotifierService>();

        // Local Services
        services.AddSingleton<INavigationService, NavigationService>();

        // Initializable
        services.AddSingleton<IHubService, HubService>();
        services.AddSingleton<ISidebarMenuService, SidebarMenuService>();

        // Callback Singletons
        services.AddSingleton<AppointmentCallbacks>();
        services.AddSingleton<NoteCallbacks>();
        services.AddSingleton<RequestCallbacks>();
    }
}