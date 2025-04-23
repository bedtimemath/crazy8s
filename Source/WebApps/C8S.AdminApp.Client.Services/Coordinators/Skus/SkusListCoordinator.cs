using Microsoft.Extensions.Logging;
using SC.Common.Helpers.Base;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.PubSub.Services;

namespace C8S.AdminApp.Client.Services.Coordinators.Skus;

public sealed class SkusListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    //private readonly ILogger<SkusListCoordinator> _logger = loggerFactory.CreateLogger<SkusListCoordinator>();
    #endregion
}
