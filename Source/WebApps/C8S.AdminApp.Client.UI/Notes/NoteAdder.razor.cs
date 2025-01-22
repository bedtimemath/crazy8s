using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public sealed partial class NoteAdder: BaseOwningComponent<NoteAdderCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<NoteAdder> Logger { get; set; } = null!; 
    #endregion

    #region Component Parameters
    [Parameter]
    public RequestDetailsCoordinator RequestCoordinator { get; set; } = null!;
    #endregion

    #region Private Variables
    private bool _isBusy = false;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        RequestCoordinator.DetailsUpdated += HandleDetailsUpdated;

        base.OnInitialized();
    }

    public void Dispose()
    {
        RequestCoordinator.DetailsUpdated -= HandleDetailsUpdated;
    }
    #endregion
    
    #region Event Handlers
    private void HandleDetailsUpdated(object? sender, EventArgs e)
    {
        var requestId = RequestCoordinator.Details?.RequestId ?? 0;
        if (requestId <= 0) return;

        Service.NotesSource = NoteReference.Request;
        Service.SourceId = requestId;
    }

    private async Task AddNoteAsync()
    {
        try
        {
            _isBusy = true;
            StateHasChanged();

            await Service.AddNote();
        }
        finally
        {
            _isBusy = false;
            StateHasChanged();
        }
    }
    #endregion
}
