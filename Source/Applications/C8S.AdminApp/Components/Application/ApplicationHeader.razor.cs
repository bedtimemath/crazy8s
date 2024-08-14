using C8S.AdminApp.Services;
using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Application;

public partial class ApplicationHeader : BaseRazorComponent
{
    [Inject]
    public ILogger<ApplicationHeader> Logger { get; set; } = default!;

    [Inject]
    public HistoryService HistoryService { get; set; } = default!;

    [Parameter]
    public ApplicationDTO? Application { get; set; }

    private void HandleCloseClicked()
    {
        try
        {
            if (Application == null) return;

            HistoryService.Remove(Application);
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }
}