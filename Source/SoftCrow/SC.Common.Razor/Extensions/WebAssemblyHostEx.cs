using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SC.Common.Razor.Navigation.Services;

namespace SC.Common.Razor.Extensions;

public static class WebAssemblyHostEx
{
    public static WebAssemblyHost UseNavigationServices(this WebAssemblyHost host)
    {
        var serviceProvider = host.Services;

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        navigationService.Initialize(serviceProvider);

        return host;
    }
}