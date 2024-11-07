using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class SkuFilter: BaseFilter
{
    public SkuStatus? Status { get; set; }
}