using C8S.Common.Helpers.PassThrus;
using C8S.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.Common.Helpers.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddCommonHelpers(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeHelper, PassthruDateTimeHelper>();
        services.AddTransient<IRandomizer, PassthruRandomizer>();

        return services;
    }
}