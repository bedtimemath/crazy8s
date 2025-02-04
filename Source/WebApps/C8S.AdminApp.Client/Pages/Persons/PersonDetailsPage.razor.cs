using C8S.AdminApp.Client.Services.Coordinators.Persons;
using Microsoft.AspNetCore.Components;
using SC.Common.Client.Base;

namespace C8S.AdminApp.Client.Pages.Persons;

public sealed partial class PersonDetailsPage :
    BaseCoordinatedPage<PersonDetailsCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<PersonDetailsPage> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public int PersonId { get; set; }
    #endregion

    #region Private Variables
    private bool _isBusy = false;
    #endregion

    #region Component LifeCycle
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (!RendererInfo.IsInteractive) return;

        _isBusy = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Service.SetIdAsync(PersonId);
        }
        finally
        {
            _isBusy = false;
            await InvokeAsync(StateHasChanged);
        }
    } 
    #endregion
}