using C8S.Domain.Features.Requests.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Commands;

public record RequestUpdateAppointmentCommand: ICQRSCommand<WrappedResponse<RequestDetails>>
{
    public int RequestId { get; init; }
    public DateTimeOffset? FullSlateAppointmentStartsOn { get; init; }
}