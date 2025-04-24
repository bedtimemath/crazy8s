using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Interfaces;
using SC.Audit.EFCore.Contexts;
using SC.Audit.EFCore.Interceptors;

namespace SC.Audit.EFCore.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddSCAuditContext(this IServiceCollection services,
        string? connectionString = null, ILoggerFactory? loggerFactory = null)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new NotImplementedException();
        
        services.AddTransient<IAuditInterceptor, AuditInterceptor>();

        services.AddDbContextFactory<SCAuditContext>((sp, config) =>
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