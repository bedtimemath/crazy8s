using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public partial class NoteDisplayer : BaseRazorComponent
{
    [Inject]
    public DialogService DialogService { get; set; } = null!;

    [Parameter]
    public NoteDetails Details { get; set; } = null!;

    [Parameter]
    public NotesListEditorCoordinator Coordinator { get; set; } = null!;

    private async Task DeleteNote(int noteId)
    {
        if (!(await DialogService
                .Confirm($"This will permanently delete the note written by '{Details.Author}'" +
                         $" on {Details.CreatedOn:M/d/yy} at {Details.CreatedOn:h:mm tt}.\r\nIs that okay?", "Confirm Delete?", 
                new ConfirmOptions() { CancelButtonText = "No, Keep It", OkButtonText = "Yes, Delete It"}) ?? false))
            return;

        await Coordinator.DeleteNote(noteId);
    }
    private async Task EditNote(int noteId)
    {
        await Coordinator.EditNote(noteId);
    }
}