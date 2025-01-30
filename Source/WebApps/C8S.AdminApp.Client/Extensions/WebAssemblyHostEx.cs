using C8S.AdminApp.Client.Services.Navigation;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Extensions;

public static class WebAssemblyHostEx
{
    public static WebAssemblyHost SetUpCQRSService(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var cqrsService = serviceProvider.GetRequiredService<ICQRSService>();

        cqrsService.RegisterCommand(typeof(OpenPageCommand), typeof(INavigationService));

        return host;
    }
}