using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Navigation;
using SC.Common.Client.Services;

namespace C8S.AdminApp.Client.Extensions;

public static class ServiceCollectionEx
{
    public static void AddLocalServices(
        this IServiceCollection services)
    {
        services.AddSingleton<INavigationService>(sp => 
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var service = new NavigationService(loggerFactory);
                service.Initialize(sp);
                return service;
            });
        services.AddSingleton<IPubSubService>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var service = new AdminPubSubService(loggerFactory);
            return service;
        });
        //services.AddSingleton<INavigationService>(sp =>
        //{
        //    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        //    var service = new NavigationService(loggerFactory);
        //    return service;
        //});
        //services.AddSingleton(sp => sp.GetRequiredService<IPubSubService>());
    }
}