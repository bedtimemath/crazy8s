using C8S.Domain.Features.Requests.Enums;
using C8S.Domain.Features.Requests.Models;
using SC.Messaging.Abstractions.Base;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestsListQuery : BaseListQuery<RequestListItem>
{
    public DateOnly? SubmittedAfter { get; init; }
    public DateOnly? SubmittedBefore { get; init; }
    public bool? HasCoachCall { get; init; }
    public IList<RequestStatus>? Statuses { get; init; }
}