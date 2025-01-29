using C8S.Domain.Features.Requests.Enums;

namespace C8S.Domain.Features.Requests.Models;

public abstract record RequestBase
{
    public int RequestId { get; init; }

    public RequestStatus Status { get; init; }
    
    public string PersonLastName { get; init; } = null!;
    public string PersonEmail { get; init; } = null!;
}