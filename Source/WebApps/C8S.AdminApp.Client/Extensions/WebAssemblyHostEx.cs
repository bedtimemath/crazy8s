using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.AdminApp.Client.Services.Menu.Services;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Services;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Extensions;

public static class WebAssemblyHostEx
{
    public static async Task<WebAssemblyHost> SetUpInitializableServices(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        // ASYNC SETUP
        var hubService = serviceProvider.GetRequiredService<IHubService>();
        await hubService.InitializeAsync(serviceProvider);

        // SYNC SETUP
        var sidebarMenuService = serviceProvider.GetRequiredService<ISidebarMenuService>();
        sidebarMenuService.Initialize(serviceProvider);

        return host;
    }
    public static WebAssemblyHost SetUpCQRSService(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var cqrsService = serviceProvider.GetRequiredService<ICQRSService>();

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        cqrsService.RegisterCommand<NavigationCommand>(navigationService.Handle);

        var sidebarMenuService = serviceProvider.GetRequiredService<ISidebarMenuService>();
        cqrsService.RegisterQuery<MenuGroupsQuery, DomainResponse<IEnumerable<MenuGroup>>>(sidebarMenuService.Handle);
        cqrsService.RegisterQuery<MenuItemsQuery, DomainResponse<IEnumerable<MenuItem>>>(sidebarMenuService.Handle);

        var requestsCallbacks = serviceProvider.GetRequiredService<RequestCallbacks>();
        cqrsService.RegisterQuery<RequestsListQuery, DomainResponse<RequestsListResults>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestDetailsQuery, DomainResponse<RequestDetails?>>(requestsCallbacks.Handle);
        cqrsService.RegisterCommand<RequestUpdateAppointmentCommand, DomainResponse<RequestDetails>>(requestsCallbacks.Handle);

        var appointmentCallbacks = serviceProvider.GetRequiredService<AppointmentCallbacks>();
        cqrsService.RegisterQuery<AppointmentDetailsQuery, DomainResponse<AppointmentDetails?>>(appointmentCallbacks.Handle);

        var noteCallbacks = serviceProvider.GetRequiredService<NoteCallbacks>();
        cqrsService.RegisterQuery<NotesListQuery, DomainResponse<NotesListResults>>(noteCallbacks.Handle);
        cqrsService.RegisterQuery<NoteDetailsQuery, DomainResponse<NoteDetails?>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteAddCommand, DomainResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteUpdateCommand, DomainResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteDeleteCommand, DomainResponse>(noteCallbacks.Handle);


        return host;
    }
}