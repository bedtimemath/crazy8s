using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Navigation;
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
    public static async Task<WebAssemblyHost> UseHubServiceAsync(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var hubService = serviceProvider.GetRequiredService<IHubService>();
        await hubService.InitializeAsync(serviceProvider);

        return host;
    }
    public static WebAssemblyHost UseCQRSService(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var cqrsService = serviceProvider.GetRequiredService<ICQRSService>();

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        cqrsService.RegisterCommand<NavigationCommand>(navigationService.Handle);

        var requestsCallbacks = serviceProvider.GetRequiredService<RequestCallbacks>();
        cqrsService.RegisterQuery<RequestsListQuery, BackendResponse<RequestsListResults>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestDetailsQuery, BackendResponse<RequestDetails?>>(requestsCallbacks.Handle);
        cqrsService.RegisterCommand<RequestUpdateAppointmentCommand, BackendResponse<RequestDetails>>(requestsCallbacks.Handle);

        var appointmentCallbacks = serviceProvider.GetRequiredService<AppointmentCallbacks>();
        cqrsService.RegisterQuery<AppointmentDetailsQuery, BackendResponse<AppointmentDetails?>>(appointmentCallbacks.Handle);

        var noteCallbacks = serviceProvider.GetRequiredService<NoteCallbacks>();
        cqrsService.RegisterQuery<NotesListQuery, BackendResponse<NotesListResults>>(noteCallbacks.Handle);
        cqrsService.RegisterQuery<NoteDetailsQuery, BackendResponse<NoteDetails?>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteAddCommand, BackendResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteUpdateCommand, BackendResponse<NoteDetails>>(noteCallbacks.Handle);
        cqrsService.RegisterCommand<NoteDeleteCommand, BackendResponse>(noteCallbacks.Handle);


        return host;
    }
}