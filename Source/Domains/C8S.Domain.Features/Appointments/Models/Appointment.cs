namespace C8S.Domain.Features.Appointments.Models;

public record Appointment
{
    public int AppointmentId { get; init; }
    public DateTimeOffset StartsOn { get; init; }
    public string StatusString { get; init; } = null!;
    public bool Deleted { get; init; }
    public DateTimeOffset CreatedOn { get; init; }
}