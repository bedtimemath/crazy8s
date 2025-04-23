using C8S.WordPress.Abstractions.Models;
using SC.Common.Helpers.CQRS.Base;

namespace C8S.WordPress.Abstractions.Queries;

public record WPUsersListQuery: BaseListQuery<WPUserDetails>
{
    public IEnumerable<string> IncludeRoles { get; init; } = [];
}