using C8S.Applications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Applications.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ApplicationService>();

        return services;
    }
}