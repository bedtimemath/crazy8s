using C8S.AdminApp.Client.Services.Callbacks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SC.Common.Helpers.Base;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientCoordinators(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Scoped Coordinators
        var coordinatorTypes = assembly.GetTypes()
            .Where(t => typeof(ICoordinator).IsAssignableFrom(t) &&
                        !t.IsAbstract)
            .ToList();
        foreach (var type in coordinatorTypes)
            services.AddScoped(type);

        // Callback Singletons
        var callbackTypes = assembly.GetTypes()
            .Where(t => typeof(ICallbacks).IsAssignableFrom(t) &&
                        !t.IsAbstract)
            .ToList();
        foreach (var type in callbackTypes)
            services.AddSingleton(type);

        return services;
    }
}