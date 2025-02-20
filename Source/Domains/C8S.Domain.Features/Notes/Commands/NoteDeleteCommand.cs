using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteDeleteCommand: ICQRSCommand<WrappedResponse>
{
    public int NoteId { get; init; }
}