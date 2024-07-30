using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.Filters;

public class OrganizationFilter: BaseFilter
{
    public OrganizationType? Type { get; set; }
}