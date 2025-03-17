using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Skus.Enums;
using SC.Messaging.Abstractions.Base;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsWithOrdersListQuery : BaseListQuery<PersonWithOrders>
{
    public List<SkuYearOption> SkuYears { get; init; } = [];
}