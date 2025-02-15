using C8S.WordPress.Abstractions.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Queries;

public record WordPressUsersListQuery: ICQRSQuery<DomainResponse<WPUsersListResults>>
{
    public IEnumerable<string> IncludeRoles { get; init; } = [];
    public IEnumerable<string> ExcludeRoles { get; init; } = [];
}