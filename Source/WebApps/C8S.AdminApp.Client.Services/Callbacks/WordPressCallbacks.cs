using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class WordPressCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<WPUsersListQuery, WrappedListResponse<WPUserDetails>>,
        // COMMANDS
        ICQRSCommandHandler<WPUserAddCommand, WrappedResponse<WPUserDetails>>,
        ICQRSCommandHandler<WPUserDeleteCommand, WrappedResponse>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<WPUserDetails>> Handle(
        WPUsersListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<WPUserDetails>("POST", "wordpress/users",
                payload: query, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling WordPress users list request: {@Query}", query);
            return WrappedListResponse<WPUserDetails>.CreateFailureResponse(exception);
        }
    }
    public async Task<WrappedListResponse<WPRoleDetails>> Handle(
        WPRolesListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<WPRoleDetails>("POST", "wordpress/roles",
                payload: query, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling WordPress roles list request: {@Query}", query);
            return WrappedListResponse<WPRoleDetails>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<WrappedResponse<WPUserDetails>> Handle(
        WPUserAddCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<WPUserDetails>("PUT", "wordpress/users", payload:command, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error adding WordPress user: {@Command}", command);
            return WrappedResponse<WPUserDetails>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<WrappedResponse<WPUserDetails>> Handle(
        WPUserUpdateRolesCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<WPUserDetails>("PATCH", $"wordpress/users/{command.Id}", payload:command, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error adding WordPress user: {@Command}", command);
            return WrappedResponse<WPUserDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse> Handle(
        WPUserDeleteCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendNoReturn("DELETE", $"wordpress/users/{command.Id}", token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error deleting WordPress user: {@Command}", command);
            return WrappedResponse.CreateFailureResponse(exception);
        }
    }
    #endregion
}