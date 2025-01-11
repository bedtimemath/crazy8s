using SC.Common.Attributes;

namespace C8S.Domain.Features.Requests.Enums;

public enum RequestStatus
{
    [Label("Received")]
    Received,
    [Label("Pending")]
    Pending,
    [Label("Approved")]
    Approved,
    [Label("Denied")]
    Denied,
    [Label("Deleted")]
    Deleted,
    [Label("Future")]
    Future
}