using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public sealed partial class NotesList : BaseOwningComponent<NotesListCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<NotesList> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public RequestDetailsCoordinator RequestCoordinator { get; set; } = null!;
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
        var requestId = RequestCoordinator.Details?.RequestId ?? 0;
        if (requestId <= 0) return;

        Service.NotesSource = NoteReference.Request;
        Service.SourceId = requestId;

        Task.Run(Service.Refresh);
    }
    
    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();

    #endregion
}
