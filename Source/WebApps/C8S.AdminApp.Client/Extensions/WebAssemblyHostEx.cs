using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.AdminApp.Client.Services.Menu.Services;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SC.Common.Client.Navigation.Commands;
using SC.Common.Client.Navigation.Queries;
using SC.Common.Client.Navigation.Services;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Extensions;

public static class WebAssemblyHostEx
{
    public static async Task<WebAssemblyHost> InitializeServices(this WebAssemblyHost host)
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
        cqrsService.RegisterQuery<CurrentUrlQuery, DomainResponse<string>>(navigationService.Handle);

        var sidebarMenuService = serviceProvider.GetRequiredService<ISidebarMenuService>();
        cqrsService.RegisterQuery<MenuGroupsQuery, DomainResponse<IEnumerable<MenuGroup>>>(sidebarMenuService.Handle);
        cqrsService.RegisterQuery<MenuSinglesQuery, DomainResponse<IEnumerable<MenuSingle>>>(sidebarMenuService.Handle);
        cqrsService.RegisterQuery<MenuItemsQuery, DomainResponse<IEnumerable<MenuItem>>>(sidebarMenuService.Handle);

        var requestsCallbacks = serviceProvider.GetRequiredService<RequestCallbacks>();
        cqrsService.RegisterQuery<RequestsListQuery, DomainResponse<RequestsListResults>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestTitleQuery, DomainResponse<string?>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestDetailsQuery, DomainResponse<RequestDetails?>>(requestsCallbacks.Handle);
        cqrsService.RegisterCommand<RequestUpdateAppointmentCommand, DomainResponse<RequestDetails>>(requestsCallbacks.Handle);

        var personsCallbacks = serviceProvider.GetRequiredService<PersonCallbacks>();
        cqrsService.RegisterQuery<PersonsListQuery, DomainResponse<PersonsListResults>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonTitleQuery, DomainResponse<string?>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonDetailsQuery, DomainResponse<PersonDetails?>>(personsCallbacks.Handle);

        var appointmentCallbacks = serviceProvider.GetRequiredService<AppointmentCallbacks>();
        cqrsService.RegisterQuery<AppointmentDetailsQuery, DomainResponse<AppointmentDetails?>>(appointmentCallbacks.Handle);

        var noteCallbacks = serviceProvider.GetRequiredService<NoteCallbacks>();
        cqrsService.RegisterQuery<NotesListQuery, DomainResponse<NotesListResults>>(noteCallbacks.Handle);
        cqrsService.RegisterQuery<NoteDetailsQuery, DomainResponse<NoteDetails?>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteAddCommand, DomainResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteUpdateCommand, DomainResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteDeleteCommand, DomainResponse>(noteCallbacks.Handle);

        var wordPressCallbacks = serviceProvider.GetRequiredService<WordPressCallbacks>();
        cqrsService.RegisterQuery<WordPressUsersListQuery, DomainResponse<WPUsersListResults>>(wordPressCallbacks.Handle);
        cqrsService.RegisterCommand<WordPressUserAddCommand, DomainResponse<WordPressUserDetails>>(wordPressCallbacks.Handle);

        return host;
    }
}