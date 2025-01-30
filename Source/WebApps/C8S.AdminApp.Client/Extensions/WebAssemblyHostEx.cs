using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Navigation;
using C8S.AdminApp.Client.Services.Pages;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Extensions;

public static class WebAssemblyHostEx
{
    public static WebAssemblyHost SetUpCQRSService(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var cqrsService = serviceProvider.GetRequiredService<ICQRSService>();

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        cqrsService.RegisterCommand<OpenPageCommand>(navigationService.Handle);
        cqrsService.RegisterCommand<ClosePageCommand>(navigationService.Handle);

        var requestsCallbacks = serviceProvider.GetRequiredService<RequestCallbacks>();
        cqrsService.RegisterQuery<RequestsListQuery, BackendResponse<RequestsListResults>>(requestsCallbacks.Handle);
        cqrsService.RegisterQuery<RequestDetailsQuery, BackendResponse<RequestDetails?>>(requestsCallbacks.Handle);

        return host;
    }
}