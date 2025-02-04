using C8S.Domain.Features.Persons.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsListQuery : ICQRSQuery<DomainResponse<PersonsListResults>>
{
    public int? StartIndex { get; init; }
    public int? Count { get; init; }
    public string? Query { get; init; }
    public string? SortDescription { get; init; }
}