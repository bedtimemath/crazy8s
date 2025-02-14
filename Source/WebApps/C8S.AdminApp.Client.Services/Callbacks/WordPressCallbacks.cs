﻿using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class WordPressCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // COMMANDS
        ICQRSCommandHandler<WordPressUserAddCommand, DomainResponse<WordPressUser>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Commands
    public async Task<DomainResponse<WordPressUser>> Handle(
        WordPressUserAddCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<WordPressUser>("PUT", "wordpress", 
                payload:command, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error adding WordPress user: {@Command}", command);
            return DomainResponse<WordPressUser>.CreateFailureResponse(exception);
        }
    }
    #endregion
}