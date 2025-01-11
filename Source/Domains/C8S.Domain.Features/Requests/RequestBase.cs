using C8S.Domain.Features.Requests.Enums;

namespace C8S.Domain.Features.Requests;

public class RequestBase
{
    #region Id Property
    public int RequestId { get; set; }
    public RequestStatus Status { get; set; }
    public string ApplicantLastName { get; set; } = default!;
    public string ApplicantEmail { get; set; } = default!;
    #endregion
}
