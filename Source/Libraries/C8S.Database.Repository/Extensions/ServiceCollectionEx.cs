using C8S.Database.Repository.Profiles;
using C8S.Database.Repository.Repositories;
using C8S.Domain.EFCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace C8S.Database.Repository.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddC8SRepository(this IServiceCollection services,
        string? connectionString = null, ILoggerFactory? loggerFactory = null)
    {
        // set up automapper (CoachProfile is used arbitrarily here)
        services.AddAutoMapper(typeof(CoachProfile).Assembly);

        // then add the context and the repository
        services.AddC8SDbContext(connectionString, loggerFactory);
        services.AddScoped<C8SRepository>();

        return services;
    }
}