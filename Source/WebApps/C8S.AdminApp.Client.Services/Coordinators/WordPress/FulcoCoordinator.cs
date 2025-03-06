using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.WordPress;

public sealed class WordPressCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<WordPressCoordinator> _logger = loggerFactory.CreateLogger<WordPressCoordinator>();
    #endregion
}
