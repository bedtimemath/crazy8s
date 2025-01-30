using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteDeleteCommand: ICQRSCommand<BackendResponse>
{
    public int NoteId { get; init; }
}