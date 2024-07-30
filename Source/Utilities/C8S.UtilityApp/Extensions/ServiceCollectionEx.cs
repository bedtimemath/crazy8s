using C8S.UtilityApp.Profiles;
using C8S.UtilityApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.UtilityApp.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddOldSystemServices(this IServiceCollection services,
        string connectionString)
    {
        // then add the database context, with its configuration
        services.AddSingleton<OldSystemService>(x => 
            ActivatorUtilities.CreateInstance<OldSystemService>(x, connectionString));

        // set up automapper (OrganizationProfile is used arbitrarily here)
        services.AddAutoMapper(typeof(OrganizationProfile).Assembly);

        return services;
    }
}