using C8S.Domain.Features.Requests.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Requests.Commands;

public record RequestUpdateAppointmentCommand: IRequest<BackendResponse<RequestDetails>>
{
    public int RequestId { get; init; }
    public DateTimeOffset? FullSlateAppointmentStartsOn { get; init; }
}