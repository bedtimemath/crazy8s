using C8S.Domain.Features.Notes.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Notes.Queries;

public record NoteDetailsQuery : ICQRSQuery<WrappedResponse<NoteDetails?>>
{
    public int NoteId { get; init; }
}