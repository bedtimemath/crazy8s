using System.Text.Json;
using C8S.AdminApp.Client.Base;
using C8S.Common.Extensions;
using C8S.Domain.Enums;
using C8S.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using SC.Common.Interfaces;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.Components.Listers;

public partial class ApplicationsLister : BaseRazorComponent
{
    private const int TotalApplications = 500;

    [Inject]
    public ILogger<ApplicationsLister> Logger { get; set; } = default!;

    [Inject]
    public IRandomizer Randomizer { get; set; } = default!;

    private Virtualize<ApplicationBase>? _listerComponent;
    private List<ApplicationBase> _applications = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        for (int index = 0; index < TotalApplications; index++)
        {
            _applications.Add(new ApplicationBase()
            {
                ApplicationId = index,
                Status = (ApplicationStatus)Randomizer.GetIntLessThan(6),
                ApplicantEmail = String.Empty.AppendRandomAlphaOnly(),
                ApplicantLastName = String.Empty.AppendRandomAlphaOnly()
            });
        }
    }

    public async Task Reload()
    {
        if (_listerComponent != null)
            await _listerComponent.RefreshDataAsync();
    }

    private async ValueTask<ItemsProviderResult<ApplicationBase>>
        GetRows(ItemsProviderRequest request)
    {
        var results = _applications.Skip(request.StartIndex).Take(request.Count).ToList();

        await Task.Delay(0);

        Logger.LogInformation("{Index}:{Count}, {@Json}",
            request.StartIndex, request.Count,
            JsonSerializer.Serialize(results.Select(r => r.ApplicationId).ToList()));

        return new ItemsProviderResult<ApplicationBase>(results, 1000);
    }
}