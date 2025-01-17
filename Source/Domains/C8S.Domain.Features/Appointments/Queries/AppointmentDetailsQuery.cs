using C8S.Domain.Features.Appointments.Models;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Features.Appointments.Queries;


public class AppointmentDetailsQuery : IRequest<BackendResponse<AppointmentDetails?>>
{
    public int AppointmentId { get; set; }
}