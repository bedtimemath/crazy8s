using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.Filters;

public class OrderFilter: BaseFilter
{
    public OrderStatus? Status { get; set; }
}