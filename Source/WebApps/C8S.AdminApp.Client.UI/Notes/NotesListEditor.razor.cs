using System.Diagnostics;
using System.Text.Json;
using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.Services.Coordinators.Requests;
using C8S.AdminApp.Client.UI.Common;
using C8S.AdminApp.Common.Interfaces;
using C8S.Domain;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radzen;
using SC.Audit.Abstractions.Models;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public sealed partial class NotesListEditor: BaseCoordinatedComponent<NotesListEditorCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<NotesListEditor> Logger { get; set; } = null!; 

    [Inject]
    public IPubSubService PubSubService { get; set; } = null!;

    [Inject]
    public DialogService DialogService { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public RequestDetailsCoordinator RequestCoordinator { get; set; } = null!;
    #endregion
    
    #region Component LifeCycle
    protected override void OnInitialized()
    {
        RequestCoordinator.DetailsLoaded += HandleDetailsLoaded;
        Service.ListUpdated += HandleListUpdated;
        Service.IsBusyChanged += HandleIsBusyChanged;

        PubSubService.Subscribe<DataChange>(HandleDataChangeNotification);

        base.OnInitialized();
    }

    public void Dispose()
    {
        RequestCoordinator.DetailsLoaded -= HandleDetailsLoaded;
        Service.ListUpdated -= HandleListUpdated;
        Service.IsBusyChanged -= HandleIsBusyChanged;
    }
    #endregion
    
    #region Event Handlers

    private async Task HandleAddNoteClicked()
    {
        var dialogResponse = await DialogService.OpenAsync<NoteEditorDialog>("Add Note",
            new Dictionary<string, object>() { { "InitialContent", String.Empty } },
            RadzenDialogOptions.Standard);
        if (dialogResponse is not string htmlContent) return;

        await Service.AddNote(htmlContent);
    }

    private void HandleDetailsLoaded(object? sender, EventArgs e)
    {
        var requestId = RequestCoordinator.Details?.RequestId ?? 0;
        if (requestId <= 0) return;

        Service.NotesSource = NoteReference.Request;
        Service.SourceId = requestId;

        Task.Run(Service.RefreshNotesList);
    }

    private async Task HandleDataChangeNotification(DataChange dataChange)
    {
        if (dataChange is not
            {
                EntityName: C8SConstants.Entities.Note,
                EntityState: EntityState.Added or EntityState.Deleted,
                JsonDetails: not null
            }) 
            return;

        var noteDetails = 
            JsonSerializer.Deserialize<NoteDetails>(dataChange.JsonDetails) ??
            throw new UnreachableException($"Could not deserialize JsonDetails to NoteDetails: {dataChange.JsonDetails}");
        if (noteDetails.ParentId != Service.SourceId) return;

        await Service.RefreshNotesList();
    }
    
    private void HandleListUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
    private void HandleIsBusyChanged(object? sender, EventArgs e) => 
        StateHasChanged();

    #endregion
}
