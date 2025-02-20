using C8S.Domain.Features.Notes.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteUpdateCommand: ICQRSCommand<WrappedResponse<NoteDetails>>
{
    public int NoteId { get; init; }
    public string Content { get; init; } = null!;
}