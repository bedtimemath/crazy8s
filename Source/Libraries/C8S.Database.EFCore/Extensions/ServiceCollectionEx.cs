using C8S.Database.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace C8S.Database.EFCore.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddC8SDbContext(this IServiceCollection services,
        string? connectionString = null, ILoggerFactory? loggerFactory = null)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new NotImplementedException();

        // then add the database context, with its configuration
        services.AddDbContextFactory<C8SDbContext>(config =>
        {
            config.UseSqlServer(connectionString);
            if (loggerFactory != null)
            {
                config.UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging(true);
            }
        });

        return services;
    }
}