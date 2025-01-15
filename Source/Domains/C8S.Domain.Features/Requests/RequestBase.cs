using C8S.Domain.Features.Requests.Enums;

namespace C8S.Domain.Features.Requests;

public class RequestBase
{
    #region Id Property
    public int RequestId { get; set; }
    public RequestStatus Status { get; set; }
    public string PersonLastName { get; set; } = null!;
    public string PersonEmail { get; set; } = null!;
    #endregion
}
