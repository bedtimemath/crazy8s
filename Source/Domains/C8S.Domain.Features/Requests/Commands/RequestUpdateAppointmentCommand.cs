using C8S.Domain.Features.Requests.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Commands;

public record RequestUpdateAppointmentCommand: ICQRSQuery<BackendResponse<RequestDetails>>
{
    public int RequestId { get; init; }
    public DateTimeOffset? FullSlateAppointmentStartsOn { get; init; }
}