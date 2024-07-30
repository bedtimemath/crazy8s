using C8S.Common.Razor.Base;
using C8S.Database.Repository.Repositories;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Home : BaseRazorPage
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public C8SRepository Repository { get; set; } = default!;

    private int? _totalApplications = null;
    private int? _totalCoaches = null;
    private int? _totalOrganizations = null;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;

        try
        {
            _totalApplications = await Repository.GetApplicationsCount();
            _totalCoaches = await Repository.GetCoachesCount();
            _totalOrganizations = await Repository.GetOrganizationsCount();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await HandleExceptionRaisedAsync(ex).ConfigureAwait(false);
        }
    }
}