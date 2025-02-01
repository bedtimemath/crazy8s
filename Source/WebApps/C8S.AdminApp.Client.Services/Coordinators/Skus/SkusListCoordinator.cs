using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Skus;

public sealed class SkusListCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService): BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SkusListCoordinator> _logger = loggerFactory.CreateLogger<SkusListCoordinator>();
    #endregion
}
