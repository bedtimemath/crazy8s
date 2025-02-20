using C8S.Domain.Features.Appointments.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Appointments.Queries;

public record AppointmentDetailsQuery : ICQRSQuery<WrappedResponse<AppointmentDetails?>>
{
    public long AppointmentId { get; init; }
}