using Microsoft.Extensions.DependencyInjection;
using C8S.DrawDown.Services;

namespace C8S.DrawDown.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddDrawDownServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<DrawDownService>();

        return services;
    }
}