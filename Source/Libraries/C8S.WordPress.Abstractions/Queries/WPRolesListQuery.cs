using C8S.WordPress.Abstractions.Models;
using SC.Messaging.Abstractions.Base;

namespace C8S.WordPress.Abstractions.Queries;

public record WPRolesListQuery: BaseListQuery<WPRoleDetails>
{
}