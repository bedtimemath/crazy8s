using C8S.AdminApp.Client.Services.Coordinators.Appointments;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
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

        services.AddScoped<AppointmentDisplayerCoordinator>();
        services.AddScoped<RequestsListCoordinator>();
        services.AddScoped<RequestDetailsCoordinator>();

        return services;
    }
}