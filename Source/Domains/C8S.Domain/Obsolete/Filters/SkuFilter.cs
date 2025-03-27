using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class SkuFilter: BaseFilter
{
    public OfferStatus? Status { get; set; }
}