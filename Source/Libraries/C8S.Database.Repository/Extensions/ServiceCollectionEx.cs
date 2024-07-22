using C8S.Database.EFCore.Extensions;
using C8S.Database.Repository.Profiles;
using C8S.Database.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Database.Repository.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddC8SRepository(this IServiceCollection services,
        string? connectionString = null)
    {
        // set up automapper (CoachProfile is used arbitrarily here)
        services.AddAutoMapper(typeof(CoachProfile).Assembly);

        // then add the context and the repository
        services.AddC8SDbContext(connectionString);
        services.AddScoped<C8SRepository>();

        return services;
    }
}