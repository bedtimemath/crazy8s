using C8S.AdminApp.Client.Services.Coordinators.Appointments;
using C8S.AdminApp.Client.Services.Coordinators.Fulco;
using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Persons;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientCoordinators(
        this IServiceCollection services)
    {
        services.AddScoped<SidebarMenuCoordinator>();
        services.AddScoped<SidebarGroupCoordinator>();
        services.AddScoped<SidebarSingleCoordinator>();
        services.AddScoped<SidebarItemListCoordinator>();
        services.AddScoped<SidebarItemCoordinator>();

        services.AddScoped<AppointmentDisplayerCoordinator>();
        
        services.AddScoped<FulcoCoordinator>();

        services.AddScoped<RequestsListCoordinator>();
        services.AddScoped<RequestDetailsCoordinator>();
        
        services.AddScoped<PersonsListCoordinator>();
        services.AddScoped<PersonDetailsCoordinator>();

        services.AddScoped<NotesListEditorCoordinator>();

        services.AddScoped<WPUserCreatorCoordinator>();
        services.AddScoped<WPCoachEditorCoordinator>();
        services.AddScoped<WPCoachListerCoordinator>();

        return services;
    }
}