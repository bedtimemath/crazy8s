﻿using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Notes.Commands;

public record NoteAddCommand: ICQRSCommand<WrappedResponse<NoteDetails>>
{
    public NoteReference Reference { get; init; }
    public int ParentId { get; init; }

    public string Content { get; init; } = null!;
}