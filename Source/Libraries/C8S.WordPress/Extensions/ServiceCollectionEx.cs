using C8S.WordPress.Abstractions.Interfaces;
using C8S.WordPress.Services;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.WordPress.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddWordPressServices(this IServiceCollection services)
    {
        services.AddScoped<IWordPressService, WordPressService>();

        return services;
    }
}