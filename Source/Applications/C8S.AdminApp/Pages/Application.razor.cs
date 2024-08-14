using C8S.AdminApp.Services;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Application : BaseRazorPage, IDisposable
{
    [Inject]
    public ILogger<Application> Logger { get; set; } = default!;

    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    [Inject]
    public HistoryService HistoryService { get; set; } = default!;

    [Parameter]
    public int ApplicationId { get; set; }

    private ApplicationDTO? _application = null;
    private OrganizationDTO? _organization = null;
    private CoachDTO? _coach = null;

    protected override void OnInitialized()
    {
        HistoryService.Changed += HandleHistoryChanged;
    }

    public void Dispose()
    {
        HistoryService.Changed -= HandleHistoryChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _application = await Repository.GetApplication(ApplicationId) ??
                       throw new Exception($"Could not find application #:{ApplicationId}");
        HistoryService.Add(_application);

        if (_application.LinkedCoachId != null) _coach = await Repository.GetCoach(_application.LinkedCoachId.Value);
        if (_application.LinkedOrganizationId != null) _organization = await Repository.GetOrganization(_application.LinkedOrganizationId.Value);
    }

    private void HandleHistoryChanged(object? sender, HistoryEventArgs eventArgs)
    {
        try
        {
            if (eventArgs.Action != HistoryAction.Remove ||
                eventArgs.Target is not ApplicationDTO application ||
                application.ApplicationId != ApplicationId) return;

            NavigationManager.NavigateTo("/applications");
        }
        catch (Exception ex)
        {
            HandleExceptionRaised(ex);
        }
    }
}