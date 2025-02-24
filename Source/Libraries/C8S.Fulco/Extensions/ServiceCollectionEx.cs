using C8S.Fulco.Services;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Fulco.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddFulcoServices(this IServiceCollection services,
        string endpoint, string userPass)
    {
        services.AddScoped<FulcoService>();
        return services;
    }
}