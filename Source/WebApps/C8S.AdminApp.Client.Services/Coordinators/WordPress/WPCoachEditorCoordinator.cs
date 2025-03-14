using System.Diagnostics;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Notifications;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.WordPress;

public sealed class WPCoachEditorCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    //private readonly ILogger<WPCoachEditorCoordinator> _logger = loggerFactory.CreateLogger<WPCoachEditorCoordinator>();

    #region Public Properties
    public WPUserDetails? Coach { get; private set; }
    #endregion

    #region Public Methods
    public void SetCoach(WPUserDetails? coach)
    {
        Coach = coach;
        Task.Run(async () => await PerformComponentRefresh());
    }
    #endregion
}