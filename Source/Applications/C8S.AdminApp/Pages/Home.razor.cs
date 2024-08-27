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
    private int? _totalClubs = null;
    private int? _totalCoaches = null;
    private int? _totalOrders = null;
    private int? _totalOrganizations = null;
    private int? _totalSkus = null;

    private bool AllHaveValues =>
        (new[] { _totalApplications, _totalClubs, _totalCoaches, _totalOrders, _totalOrganizations, _totalSkus })
        .All(b => b.HasValue);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;

        try
        {
            _totalApplications = await Repository.GetApplicationsCount();
            _totalClubs = await Repository.GetClubsCount();
            _totalCoaches = await Repository.GetCoachesCount();
            _totalOrders = await Repository.GetOrdersCount();
            _totalOrganizations = await Repository.GetOrganizationsCount();
            _totalSkus = await Repository.GetSkusCount();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await HandleExceptionRaisedAsync(ex).ConfigureAwait(false);
        }
    }
}