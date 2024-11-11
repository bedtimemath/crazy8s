using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace C8S.Domain.EFCore.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddC8SDbContext(this IServiceCollection services,
        string? connectionString = null, ILoggerFactory? loggerFactory = null)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new NotImplementedException();

        services.AddSingleton<CreateModifyInterceptor>();

        services.AddDbContextFactory<C8SDbContext>((sp, config) =>
        {
            // do the configuration
            config.UseSqlServer(connectionString);
            config.AddInterceptors(
                sp.GetRequiredService<CreateModifyInterceptor>());

            if (loggerFactory != null)
            {
                config.UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging(true);
            }
        });

        return services;
    }
}