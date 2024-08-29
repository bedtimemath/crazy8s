using C8S.Common;
using C8S.Common.Extensions;
using C8S.Common.Models;
using C8S.Database.Abstractions.Models;
using C8S.UtilityApp.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class ShowConfig(
    ILogger<ShowConfig> logger,
    IConfiguration configuration)
    : IActionLauncher
{
    public Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(ShowConfig)} ===");

        // GENERAL
        Console.WriteLine("== General ==\r\n");
        Console.WriteLine("Environment: {0}\r\n", configuration["ENVIRONMENT"]);
        
        // CONNECTIONS
        var connections = new Connections();
        configuration.GetSection(Connections.SectionName).Bind(connections);
        Console.WriteLine("AzureStorage: {0}\r\n", connections.AzureStorage?.Obscure());
        Console.WriteLine("Database: {0}\r\n", connections.Database?.Obscure());
        Console.WriteLine("OldSystem: {0}\r\n", connections.OldSystem?.Obscure());
        Console.WriteLine("ApplicationInsights: {0}\r\n", connections.ApplicationInsights?.Obscure());

        // API KEYS
        var apiKeys = new ApiKeys();
        configuration.GetSection(ApiKeys.SectionName).Bind(apiKeys);

        Console.WriteLine("== ApiKeys ==\r\n");
        Console.WriteLine("FullSlate: {0}\r\n", apiKeys.FullSlate?.Obscure());
        
        // ENDPOINTS
        var endpoints = new Endpoints();
        configuration.GetSection(Endpoints.SectionName).Bind(endpoints);

        Console.WriteLine("== Endpoints ==\r\n");
        Console.WriteLine("AzureStorage: {0}\r\n", endpoints.AzureStorage?.Obscure());
        Console.WriteLine("FullSlateApi: {0}\r\n", endpoints.FullSlateApi?.Obscure());

        logger.LogInformation("{Name}: complete.", nameof(ShowConfig));

        return Task.FromResult(0);
    }
}