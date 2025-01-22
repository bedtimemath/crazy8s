using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteAddCommand: IRequest<BackendResponse<NoteDetails>>
{
    public NoteReference Reference { get; init; }
    public int ParentId { get; init; }

    public string Content { get; init; } = null!;
}