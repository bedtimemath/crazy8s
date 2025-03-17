using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.AdminApp.Client.Services.Menu.Services;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Clubs.Queries;
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
using SC.Common.Responses;
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
        cqrsService.RegisterQuery<CurrentUrlQuery, WrappedResponse<string>>(navigationService.Handle);

        var sidebarMenuService = serviceProvider.GetRequiredService<ISidebarMenuService>();
        cqrsService.RegisterQuery<MenuGroupsQuery, WrappedResponse<IEnumerable<MenuGroup>>>(sidebarMenuService.Handle);
        cqrsService.RegisterQuery<MenuSinglesQuery, WrappedResponse<IEnumerable<MenuSingle>>>(sidebarMenuService.Handle);
        cqrsService.RegisterQuery<MenuItemsQuery, WrappedResponse<IEnumerable<MenuItem>>>(sidebarMenuService.Handle);

        var requestsCallbacks = serviceProvider.GetRequiredService<RequestCallbacks>();
        cqrsService.RegisterQuery<RequestsListQuery, WrappedListResponse<RequestListItem>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestTitleQuery, WrappedResponse<string?>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestDetailsQuery, WrappedResponse<RequestDetails?>>(requestsCallbacks.Handle);
        cqrsService.RegisterCommand<RequestUpdateAppointmentCommand, WrappedResponse<RequestDetails>>(requestsCallbacks.Handle);

        var personsCallbacks = serviceProvider.GetRequiredService<PersonCallbacks>();
        cqrsService.RegisterQuery<PersonsListQuery, WrappedListResponse<Person>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonsWithOrdersListQuery, WrappedListResponse<PersonWithOrders>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonQuery, WrappedResponse<Person?>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonWithOrdersQuery, WrappedResponse<PersonWithOrders?>>(personsCallbacks.Handle);
        cqrsService.RegisterQuery<PersonTitleQuery, WrappedResponse<string?>>(personsCallbacks.Handle);

        var appointmentCallbacks = serviceProvider.GetRequiredService<AppointmentCallbacks>();
        cqrsService.RegisterQuery<AppointmentDetailsQuery, WrappedResponse<Appointment?>>(appointmentCallbacks.Handle);

        var noteCallbacks = serviceProvider.GetRequiredService<NoteCallbacks>();
        cqrsService.RegisterQuery<NotesListQuery, WrappedListResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterQuery<NoteDetailsQuery, WrappedResponse<NoteDetails?>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteAddCommand, WrappedResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteUpdateCommand, WrappedResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteDeleteCommand, WrappedResponse>(noteCallbacks.Handle);

        var wordPressCallbacks = serviceProvider.GetRequiredService<WordPressCallbacks>();
        cqrsService.RegisterQuery<WPRolesListQuery, WrappedListResponse<WPRoleDetails>>(wordPressCallbacks.Handle);
        cqrsService.RegisterQuery<WPUsersListQuery, WrappedListResponse<WPUserDetails>>(wordPressCallbacks.Handle);
        cqrsService.RegisterCommand<WPUserAddCommand, WrappedResponse<WPUserDetails>>(wordPressCallbacks.Handle);
        cqrsService.RegisterCommand<WPUserUpdateRolesCommand, WrappedResponse<WPUserDetails>>(wordPressCallbacks.Handle);
        cqrsService.RegisterCommand<WPUserDeleteCommand, WrappedResponse>(wordPressCallbacks.Handle);

        return host;
    }
}