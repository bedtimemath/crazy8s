using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class RequestCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<RequestsListQuery, DomainResponse<RequestsListResults>>,
        ICQRSQueryHandler<RequestDetailsQuery, DomainResponse<RequestDetails?>>,
        // COMMANDS
        ICQRSCommandHandler<RequestUpdateAppointmentCommand, DomainResponse<RequestDetails>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestCallbacks> _logger = loggerFactory.CreateLogger<RequestCallbacks>();
    #endregion

    #region Queries
    public async Task<DomainResponse<RequestsListResults>> Handle(
        RequestsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<RequestsListResults>("POST", "requests", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling requests list request: {@Request}", query);
            return DomainResponse<RequestsListResults>.CreateFailureResponse(exception);
        }
    }

    public async Task<DomainResponse<RequestDetails?>> Handle(
        RequestDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<RequestDetails?>("GET", "request", query.RequestId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request details request: {@Request}", query);
            return DomainResponse<RequestDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<DomainResponse<RequestDetails>> Handle(
        RequestUpdateAppointmentCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<RequestDetails>("PATCH", "request", command.RequestId, 
                payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling update appointment command: {@Command}", command);
            return DomainResponse<RequestDetails>.CreateFailureResponse(exception);
        }
    }
    #endregion
}