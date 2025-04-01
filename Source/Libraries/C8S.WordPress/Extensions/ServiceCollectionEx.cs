using System.Diagnostics;
using AutoMapper;
using C8S.WordPress.Mapping;
using C8S.WordPress.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace C8S.WordPress.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddWordPressServices(this IServiceCollection services,
        string endpoint, string userPass)
    {
        var userPassParts = userPass.Split('|');
        if (userPassParts.Length != 2)
            throw new UnreachableException($"Could not parse ApiKeys:WPCoachesArea : {userPass}");
        var username = userPassParts[0];
        var password = userPassParts[1];
        return AddWordPressServices(services, endpoint, username, password);
    }
    public static IServiceCollection AddWordPressServices(this IServiceCollection services,
        string endpoint, string username, string password)
    {
        // set up automapper (WPProfiles is used arbitrarily here)
        services.AddAutoMapper(typeof(WPKitPageProfiles));

        services.AddScoped<WordPressService>(svc =>
            new WordPressService(
                svc.GetRequiredService<ILoggerFactory>(), 
                svc.GetRequiredService<IMapper>(),
                endpoint, username, password));

        return services;
    }
}