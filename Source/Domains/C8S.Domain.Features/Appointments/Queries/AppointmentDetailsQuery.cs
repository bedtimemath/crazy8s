using C8S.Domain.Features.Appointments.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Appointments.Queries;

public record AppointmentDetailsQuery : ICQRSQuery<BackendResponse<AppointmentDetails?>>
{
    public int AppointmentId { get; init; }
}