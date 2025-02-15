using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class WordPressCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // COMMANDS
        ICQRSCommandHandler<WordPressUserAddCommand, DomainResponse<WordPressUserDetails>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Queries
    public async Task<DomainResponse<WPUsersListResults>> Handle(
        WordPressUsersListQuery query, CancellationToken token)
    {
        try
        {
            throw new NotImplementedException();
            //return await CallBackendServer<WordPressUserDetails>("PUT", "wordpress", 
            //    payload:command, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error adding WordPress user: {@Command}", query);
            return DomainResponse<WPUsersListResults>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<DomainResponse<WordPressUserDetails>> Handle(
        WordPressUserAddCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<WordPressUserDetails>("PUT", "wordpress", 
                payload:command, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error adding WordPress user: {@Command}", command);
            return DomainResponse<WordPressUserDetails>.CreateFailureResponse(exception);
        }
    }
    #endregion
}