using C8S.AdminApp.Client.Services.Coordinators.Appointments;
using C8S.AdminApp.Client.Services.Coordinators.Menus;
using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Persons;
using C8S.AdminApp.Client.Services.Coordinators.Tickets;
using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using Microsoft.Extensions.DependencyInjection;
using SC.Common.Razor.Interfaces;
using System.Reflection;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddClientCoordinators(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var coordinatorTypes = assembly.GetTypes()
            .Where(t => typeof(ICoordinator).IsAssignableFrom(t))
            .ToList();

        foreach (var type in coordinatorTypes)
            services.AddScoped(type);

        return services;
    }
}