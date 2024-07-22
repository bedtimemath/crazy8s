using C8S.Common;
using C8S.Database.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Database.EFCore.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddC8SDbContext(this IServiceCollection services,
        string? connectionString = null)
    {
        // read the connection string from the configuration if we're not given it
        connectionString ??= services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>()
                .GetConnectionString(C8SConstants.Connections.Database);

        // then add the database context, with its configuration
        services.AddDbContextFactory<C8SDbContext>(config =>
        {
            config.UseSqlServer(connectionString);
        });

        return services;
    }
}