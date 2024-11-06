using C8S.Database.EFCore.Contexts;
using C8S.Database.EFCore.Interceptors;
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

        services.AddSingleton<AuditInterceptor>();

        services.AddDbContextFactory<C8SDbContext>((sp, config) =>
        {
            config.UseSqlServer(connectionString);
            config.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());

            if (loggerFactory != null)
            {
                config.UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging(true);
            }
        });

        return services;
    }
}