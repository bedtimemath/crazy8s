using C8S.Common;
using C8S.UtilityApp.Profiles;
using C8S.UtilityApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.UtilityApp.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddOldSystemServices(this IServiceCollection services,
        string? connectionString = null)
    {
        // read the connection string from the configuration if we're not given it
        connectionString ??= services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>()
                .GetConnectionString(C8SConstants.Connections.OldSystem) ??
                             throw new Exception($"Could not find connection string: {C8SConstants.Connections.OldSystem}");

        // then add the database context, with its configuration
        services.AddSingleton<OldSystemService>(x => 
            ActivatorUtilities.CreateInstance<OldSystemService>(x, connectionString));

        // set up automapper (OrganizationProfile is used arbitrarily here)
        services.AddAutoMapper(typeof(OrganizationProfile).Assembly);

        return services;
    }
}