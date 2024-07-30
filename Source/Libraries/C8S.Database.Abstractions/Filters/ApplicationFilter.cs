using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.Filters;

public class ApplicationFilter: BaseFilter
{
    public ApplicationStatus? Status { get; set; }
}