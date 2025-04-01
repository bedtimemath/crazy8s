using C8S.Domain.Mapping.MapProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Domain.Mapping.Extensions;

public static class ServiceCollectionEx
{
    public static void AddDomainMapping(
        this IServiceCollection services)
    {
        // PersonProfile is used arbitrarily here
        services.AddAutoMapper(typeof(PersonProfile).Assembly);
    }
}
