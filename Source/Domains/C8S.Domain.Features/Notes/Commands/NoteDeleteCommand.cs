using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteDeleteCommand: IRequest<BackendResponse>
{
    public int NoteId { get; init; }
}