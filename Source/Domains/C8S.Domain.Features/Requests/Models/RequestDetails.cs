using C8S.Domain.Enums;

namespace C8S.Domain.Features.Requests.Models;

public record RequestDetails: RequestAbstract
{
    public ApplicantType? PersonType { get; init; }
    public string? PersonPhone { get; init; }
    public string? ReferenceSource { get; init; }
    public string? ReferenceSourceOther { get; init; }
    public string? Comments { get; init; }
}
