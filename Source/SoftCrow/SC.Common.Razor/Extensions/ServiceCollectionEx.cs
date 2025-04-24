using Microsoft.Extensions.DependencyInjection;
using SC.Common.Razor.Navigation.Services;

namespace SC.Common.Razor.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddNavigationServices(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();

        return services;
    }
}