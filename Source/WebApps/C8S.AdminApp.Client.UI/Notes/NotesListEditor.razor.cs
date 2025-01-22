using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public sealed partial class NotesListEditor: BaseOwningComponent<NotesListEditorCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<NotesListEditor> Logger { get; set; } = null!; 
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
        Service.ListUpdated += HandleListUpdated;

        base.OnInitialized();
    }

    public void Dispose()
    {
        RequestCoordinator.DetailsUpdated -= HandleDetailsUpdated;
        Service.ListUpdated -= HandleListUpdated;
    }
    #endregion
    
    #region Event Handlers
    private void HandleDetailsUpdated(object? sender, EventArgs e)
    {
        Logger.LogDebug("HandleDetailsUpdated: #{Id}", RequestCoordinator.Details?.RequestId ?? 0);

        var requestId = RequestCoordinator.Details?.RequestId ?? 0;
        if (requestId <= 0) return;

        Service.NotesSource = NoteReference.Request;
        Service.SourceId = requestId;

        Task.Run(Service.Refresh);
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
    
    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();

    #endregion
}
