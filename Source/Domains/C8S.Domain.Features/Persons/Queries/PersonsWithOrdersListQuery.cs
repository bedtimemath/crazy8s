using C8S.Domain.Features.Offers.Enums;
using C8S.Domain.Features.Persons.Models;
using SC.Common.Helpers.CQRS.Base;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsWithOrdersListQuery : BaseListQuery<PersonWithOrders>
{
    public List<KitYearOption> SkuYears { get; init; } = [];
}