using C8S.AdminApp.Client.Services.Navigation;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client;

public partial class Routes(
    ILoggerFactory loggerFactory,
    IServiceProvider serviceProvider): ComponentBase
{
    private readonly ILogger<Routes> _logger = loggerFactory.CreateLogger<Routes>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;
        
        var cqrsService = serviceProvider.GetRequiredService<ICQRSService>();

        cqrsService.RegisterCommand(typeof(OpenPageCommand), typeof(INavigationService));


    }
}