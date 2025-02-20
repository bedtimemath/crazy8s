using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class RequestCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<RequestsListQuery, WrappedListResponse<RequestListItem>>,
        ICQRSQueryHandler<RequestDetailsQuery, WrappedResponse<RequestDetails?>>,
        ICQRSQueryHandler<RequestTitleQuery, WrappedResponse<string?>>,
        // COMMANDS
        ICQRSCommandHandler<RequestUpdateAppointmentCommand, WrappedResponse<RequestDetails>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestCallbacks> _logger = loggerFactory.CreateLogger<RequestCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<RequestListItem>> Handle(
        RequestsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<RequestListItem>("POST", "requests", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling requests list request: {@Request}", query);
            return WrappedListResponse<RequestListItem>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<RequestDetails?>> Handle(
        RequestDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<RequestDetails?>("GET", "requests", query.RequestId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request details request: {@Request}", query);
            return WrappedResponse<RequestDetails?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<string?>> Handle(
        RequestTitleQuery query, CancellationToken token)
    {
        try
        {
            var request = await CallBackendReturnSingle<RequestDetails?>("GET", "requests", query.RequestId, token:token);
            return request switch
            {
                { Success: false, Exception: not null } =>
                    WrappedResponse<string?>.CreateFailureResponse(request.Exception),
                { Success: false, Exception: null } =>
                    WrappedResponse<string?>.CreateFailureResponse(new SerializableException("Unknown Error")),
                { Success: true, Result: null } =>
                    WrappedResponse<string?>.CreateSuccessResponse(null),
                { Success: true, Result: not null } =>
                    WrappedResponse<string?>.CreateSuccessResponse(request.Result.PersonFullName)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request details request: {@Request}", query);
            return WrappedResponse<string?>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<WrappedResponse<RequestDetails>> Handle(
        RequestUpdateAppointmentCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<RequestDetails>("PATCH", "requests", command.RequestId, 
                payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling update appointment command: {@Command}", command);
            return WrappedResponse<RequestDetails>.CreateFailureResponse(exception);
        }
    }
    #endregion
}