using C8S.Domain.Features.Notes.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Queries;

public record NoteDetailsQuery : ICQRSQuery<DomainResponse<NoteDetails?>>
{
    public int NoteId { get; init; }
}