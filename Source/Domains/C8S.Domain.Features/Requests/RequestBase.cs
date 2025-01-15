using C8S.Domain.Features.Requests.Enums;

namespace C8S.Domain.Features.Requests;

public record RequestBase(
    int RequestId,
    RequestStatus Status,
    string PersonLastName,
    string PersonEmail
    );