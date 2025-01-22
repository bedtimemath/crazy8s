using C8S.AdminApp.Client.Services.Coordinators.Notes;
using C8S.Domain.Features.Notes.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public partial class NoteDisplayer : BaseRazorComponent
{
    [Inject]
    public ISender Sender { get; set; } = null!;

    [Parameter]
    public NoteDetails Note { get; set; } = null!;

    [Parameter]
    public NotesListCoordinator Coordinator { get; set; } = null!;

    private async Task DeleteNote(int noteId)
    {
        await Coordinator.DeleteNote(noteId);
    }
    private async Task EditNote(int noteId)
    {
        await Coordinator.EditNote(noteId);
    }
}