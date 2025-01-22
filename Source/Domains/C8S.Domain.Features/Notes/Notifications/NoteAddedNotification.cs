using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;

namespace C8S.Domain.Features.Notes.Notifications;

public record NoteAddedNotification
{
    public NoteReference ReferenceSource { get; init; }
    public int SourceId { get; init; }
    public NoteDetails Details { get; init; } = null!;
}