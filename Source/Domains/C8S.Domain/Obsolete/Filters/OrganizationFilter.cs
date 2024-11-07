using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class OrganizationFilter: BaseFilter
{
    public OrganizationType? Type { get; set; }
}