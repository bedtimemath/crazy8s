using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteDeleteCommand: ICQRSCommand<WrappedResponse>
{
    public int NoteId { get; init; }
}