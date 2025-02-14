using System.Diagnostics;
using C8S.WordPress.Services;
using Microsoft.Extensions.DependencyInjection;
using WordPressPCL;

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

        var apiClient = new WordPressClient(endpoint);
        apiClient.Auth.UseBasicAuth(username, password);

        services.AddSingleton(apiClient);
        services.AddScoped<WordPressService>();

        return services;
    }
}