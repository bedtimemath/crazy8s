using C8S.Domain.Features.Appointments.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Appointments.Queries;

public record AppointmentDetailsQuery : ICQRSQuery<WrappedResponse<Appointment?>>
{
    public long AppointmentId { get; init; }
}