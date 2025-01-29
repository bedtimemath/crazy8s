using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Queries;

public record NotesListQuery : ICQRSQuery<BackendResponse<NotesListResults>>
{
    public NoteReference NotesSource { get; init; }
    public int? SourceId { get; init; }
}