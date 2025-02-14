using System.Diagnostics;
using C8S.Domain.Features.Appointments.Models;
using C8S.FullSlate.Services;
using Microsoft.AspNetCore.Mvc;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers
{
    [ApiController]
    public class AppointmentController(
        ILoggerFactory loggerFactory,
        FullSlateService fullSlateService
        ) : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger = loggerFactory.CreateLogger<AppointmentController>();
        
        [HttpGet]
        [Route("api/[controller]/{appointmentId:int}")]
        public async Task<DomainResponse<AppointmentDetails?>> GetAppointment(int appointmentId)
        {
            try
            {
                var appointmentResponse = await fullSlateService.GetAppointment(appointmentId);
                if (!appointmentResponse.Success)
                {
                    // gather the full slate errors
                    var errorMessages = appointmentResponse.Errors?
                        .Select(e => e.ErrorMessage)
                        .ToList() ?? [];
                    if (errorMessages.Count == 0) errorMessages.Add("Unknown Error");
                    throw new Exception(String.Join("; ", errorMessages));
                }

                // we could use automapper for this when/if it gets more complicated
                var fullSlateAppointment = appointmentResponse.Data;
                var appointment = fullSlateAppointment == null ? null : new AppointmentDetails()
                    {
                        AppointmentId = fullSlateAppointment.Id,
                        StatusString = fullSlateAppointment.StatusString ??
                                       throw new UnreachableException("FullSlate Appointment missing StatusString"),
                        StartsOn = fullSlateAppointment.AtDateTime ??
                                   throw new UnreachableException("FullSlate Appointment missing AtDateTime"),
                        Deleted = fullSlateAppointment.Deleted ??
                                  throw new UnreachableException("FullSlate Appointment missing Deleted"),
                        CreatedOn = fullSlateAppointment.CreatedAt ??
                                    throw new UnreachableException("FullSlate Appointment missing Deleted")
                    };
                
                return new DomainResponse<AppointmentDetails?>() { Result = appointment };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while getting details: {Id}", appointmentId);
                return new DomainResponse<AppointmentDetails?>()
                {
                    Exception = exception.ToSerializableException()
                };
            }
        }
    }
}
