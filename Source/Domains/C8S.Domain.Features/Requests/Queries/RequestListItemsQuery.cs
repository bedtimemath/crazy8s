using C8S.Domain.Features.Requests.Enums;
using SC.Common.Interactions;
using C8S.Domain.Features.Requests.Models;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestsListQuery : ICQRSQuery<DomainResponse<RequestsListResults>>
{
    public int? StartIndex { get; init; }
    public int? Count { get; init; }
    public string? Query { get; init; }
    public string? SortDescription { get; init; }
    public DateOnly? SubmittedAfter { get; init; }
    public DateOnly? SubmittedBefore { get; init; }
    public bool? HasCoachCall { get; init; }
    public IList<RequestStatus>? Statuses { get; init; }
}