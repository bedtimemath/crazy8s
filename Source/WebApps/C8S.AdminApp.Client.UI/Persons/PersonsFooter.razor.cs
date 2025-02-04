using C8S.AdminApp.Client.Services.Coordinators.Persons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Persons;

public sealed partial class PersonsFooter: BaseRazorComponent
{
    
    [Inject]
    public ILogger<PersonsFooter> Logger { get; set; } = null!;
    
    [Parameter]
    public PersonsListCoordinator Coordinator { get; set; } = null!;

    protected override void OnInitialized()
    {
        Coordinator.ListUpdated += HandleListUpdated;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Coordinator.ListUpdated -= HandleListUpdated;
    }

    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
}
