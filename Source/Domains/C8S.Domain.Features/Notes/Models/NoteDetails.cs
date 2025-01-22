using C8S.Domain.Enums;

namespace C8S.Domain.Features.Notes.Models;

public record NoteDetails
{
    public int NoteId { get; init; }
    
    public NoteReference Reference { get; init; }
    public int ParentId { get; init; }

    public string Content { get; init; } = null!;
    public string Author { get; init; } = null!;

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}