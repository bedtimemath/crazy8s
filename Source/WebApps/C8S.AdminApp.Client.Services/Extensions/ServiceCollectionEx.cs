using C8S.AdminApp.Client.Services.Data;
using C8S.AdminApp.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ICommunicationService, CommunicationService>();

        return services;
    }
}