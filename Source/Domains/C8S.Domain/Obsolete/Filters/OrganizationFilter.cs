using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class OrganizationFilter: BaseFilter
{
    public PlaceType? Type { get; set; }
}