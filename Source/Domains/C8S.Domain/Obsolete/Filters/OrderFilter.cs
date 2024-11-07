using C8S.Domain.Enums;

namespace C8S.Domain.Obsolete.Filters;

public class OrderFilter: BaseFilter
{
    public OrderStatus? Status { get; set; }
}