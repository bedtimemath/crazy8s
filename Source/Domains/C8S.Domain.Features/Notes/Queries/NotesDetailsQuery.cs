using C8S.Domain.Features.Notes.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Notes.Queries;

public record NoteDetailsQuery : IRequest<BackendResponse<NoteDetails?>>
{
    public int NoteId { get; init; }
}