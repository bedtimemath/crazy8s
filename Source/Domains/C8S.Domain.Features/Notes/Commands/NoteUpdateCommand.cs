using C8S.Domain.Features.Notes.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteUpdateCommand: ICQRSCommand<WrappedResponse<NoteDetails>>
{
    public int NoteId { get; init; }
    public string Content { get; init; } = null!;
}