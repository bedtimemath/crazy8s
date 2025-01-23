using C8S.Domain.Features.Notes.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteUpdateCommand: IRequest<BackendResponse<NoteDetails>>
{
    public int NoteId { get; init; }
    public string Content { get; init; } = null!;
}