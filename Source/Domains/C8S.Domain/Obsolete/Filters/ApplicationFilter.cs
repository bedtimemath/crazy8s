using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class ApplicationFilter: BaseFilter
{
    public RequestStatus? Status { get; set; }
}