﻿using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Sites;

public sealed class SitesListCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService): BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SitesListCoordinator> _logger = loggerFactory.CreateLogger<SitesListCoordinator>();
    #endregion
}
