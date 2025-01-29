using C8S.Domain.Features.Notes.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteUpdateCommand: ICQRSQuery<BackendResponse<NoteDetails>>
{
    public int NoteId { get; init; }
    public string Content { get; init; } = null!;
}