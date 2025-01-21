using C8S.AdminApp.Client.Services.Coordinators.Notes;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public sealed partial class NoteEditor: BaseOwningComponent<NoteEditorCoordinator>
{
    [Inject]
    public ILogger<NoteEditor> Logger { get; set; } = null!;

    [Parameter]
    public string NotesSource { get; set; } = null!;

    [Parameter]
    public int SourceId { get; set; }
}
