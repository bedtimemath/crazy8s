using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Application : BaseRazorPage
{
    [Inject]
    public ILogger<Application> Logger { get; set; } = default!;
    
    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    [Parameter]
    public int ApplicationId { get; set; }

    private ApplicationDTO? _application = null;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _application = await Repository.GetApplication(ApplicationId) ??
                       throw new Exception($"Could not find application #:{ApplicationId}");
    }
}