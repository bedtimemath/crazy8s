using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Notes.Queries;

public record NotesListQuery : IRequest<BackendResponse<NotesListResults>>
{
    public NoteReference NotesSource { get; init; }
    public int? SourceId { get; init; }
}