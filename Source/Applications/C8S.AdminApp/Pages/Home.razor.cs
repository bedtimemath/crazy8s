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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;

        try
        {
            _totalApplications = await Repository.GetApplicationsCount();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await HandleExceptionRaisedAsync(ex).ConfigureAwait(false);
        }
    }
}