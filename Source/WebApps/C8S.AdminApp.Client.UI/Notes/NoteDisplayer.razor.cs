using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.AdminApp.Client.UI.Common;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public partial class NoteDisplayer : BaseRazorComponent
{
    [Inject]
    public ILogger<NoteDisplayer> Logger { get; set; } = null!;
    
    [Inject]
    public DialogService DialogService { get; set; } = null!;

    [Parameter]
    public NoteDetails Details { get; set; } = null!;

    [Parameter]
    public NotesListEditorCoordinator Coordinator { get; set; } = null!;

    private async Task HandleDeleteClicked(NoteDetails note)
    {
        try
        {
            if (!(await DialogService
                    .Confirm($"This will permanently delete the note written by '{Details.Author}'" +
                             $" on {Details.CreatedOn:M/d/yy} at {Details.CreatedOn:h:mm tt}.\r\nIs that okay?", "Confirm Delete?", 
                        new ConfirmOptions() { CancelButtonText = "No, Keep It", OkButtonText = "Yes, Delete It"}) ?? false))
                return;

            await Coordinator.DeleteNote(note.NoteId);
        }
        catch (Exception exc)
        {
            Logger.LogError(exc, "Could not delete note: {@Note}", note);
            // todo
            //await RaiseExceptionAsync(exc);
        }
    }
    private async Task HandleEditClicked(NoteDetails note)
    {
        try
        {
            var dialogResponse = await DialogService.OpenAsync<NoteEditorDialog>("Add Note",
                new Dictionary<string, object>() { { "InitialContent", note.Content } },
                RadzenDialogOptions.Standard);
            if (dialogResponse is not string htmlContent) return;

            if (htmlContent == note.Content) return;

            await Coordinator.UpdateNote(note with { Content = htmlContent });
        }
        catch (Exception exc)
        {
            Logger.LogError(exc, "Could not edit note: {@Note}", note);
            // todo
            //await RaiseExceptionAsync(exc);
        }
    }
}