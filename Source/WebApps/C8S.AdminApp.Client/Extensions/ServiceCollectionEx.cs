using C8S.AdminApp.Client.Services;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddCQRSService(this IServiceCollection services)
    {
        services.AddScoped<ICQRSService, CQRSService>();
    }

    public static void AddLocalServices(
        this IServiceCollection services)
    {
        //services.AddSingleton<IPubSubService>(sp =>
        //{
        //    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        //    var service = new AdminPubSubService(loggerFactory);
        //    return service;
        //});
        //services.AddSingleton<INavigationService>(sp =>
        //{
        //    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        //    var service = new NavigationService(loggerFactory);
        //    return service;
        //});
        //services.AddSingleton(sp => sp.GetRequiredService<IPubSubService>());
    }
}