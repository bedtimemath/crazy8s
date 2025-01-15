using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.Services.Data;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ICommunicationService, CommunicationService>();
        services.AddSingleton<IPagesService, PagesService>();

        services.AddScoped<RequestsListController>();
        services.AddScoped<RequestDetailsController>();

        return services;
    }
}