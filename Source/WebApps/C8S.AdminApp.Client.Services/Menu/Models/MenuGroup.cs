using C8S.Domain.Features;
using SC.Common;

namespace C8S.AdminApp.Client.Services.Menu.Models;

public record MenuGroup
{
    public DomainEntity? Entity { get; init; }
    public string Display { get; init; } = null!;
    public string IconGroup { get; init; } = SoftCrowConstants.IconGroups.Regular;
    public string IconString { get; init; } = null!;
    public string Url { get; init; } = null!;
}