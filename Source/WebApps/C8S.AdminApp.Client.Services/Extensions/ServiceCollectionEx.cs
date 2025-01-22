using C8S.AdminApp.Client.Services.Coordinators.Appointments;
using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientCoordinators(
        this IServiceCollection services)
    {
        services.AddScoped<AppointmentDisplayerCoordinator>();
        
        services.AddScoped<RequestsListCoordinator>();

        services.AddScoped<RequestDetailsCoordinator>();

        services.AddScoped<NotesListCoordinator>();

        services.AddScoped<NoteAdderCoordinator>();
        services.AddScoped<NoteEditorCoordinator>();

        return services;
    }
}