using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.Filters;

public class SkuFilter: BaseFilter
{
    public SkuStatus? Status { get; set; }
}