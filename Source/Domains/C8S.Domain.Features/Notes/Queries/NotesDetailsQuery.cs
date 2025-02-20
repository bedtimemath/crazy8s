using C8S.Domain.Features.Notes.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Queries;

public record NoteDetailsQuery : ICQRSQuery<WrappedResponse<NoteDetails?>>
{
    public int NoteId { get; init; }
}