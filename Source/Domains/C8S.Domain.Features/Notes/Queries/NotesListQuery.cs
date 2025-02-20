using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using SC.Messaging.Abstractions.Base;

namespace C8S.Domain.Features.Notes.Queries;

public record NotesListQuery : BaseListQuery<NoteDetails>
{
    public NoteReference NotesSource { get; init; }
    public int? SourceId { get; init; }
}