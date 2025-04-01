using C8S.Domain.Features.Offers.Enums;
using C8S.Domain.Features.Persons.Models;
using SC.Messaging.Abstractions.Base;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsWithOrdersListQuery : BaseListQuery<PersonWithOrders>
{
    public List<KitYearOption> SkuYears { get; init; } = [];
}