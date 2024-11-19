using System.Net.Http.Headers;
using C8S.FullSlate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.FullSlate.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddFullSlateServices(this IServiceCollection services,
        string fullSlateUrl, string fullSlateToken)
    {
        services.AddHttpClient(FullSlateService.HttpAuthName, client =>
        {
            client.BaseAddress = new Uri(fullSlateUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fullSlateToken);
        })
        //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() { Proxy = new WebProxy("http://localhost:8866")})
        ; 

        services.AddTransient<FullSlateService>();

        return services;
    }
}