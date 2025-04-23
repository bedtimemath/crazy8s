using C8S.Domain.Features.Tickets.Models;
using C8S.Domain.Features.Tickets.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class TicketCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<TicketsListQuery, WrappedListResponse<TicketListItem>> //,
        //ICQRSQueryHandler<TicketQuery, WrappedResponse<Ticket?>>,
        //ICQRSQueryHandler<TicketTitleQuery, WrappedResponse<string?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<TicketCallbacks> _logger = loggerFactory.CreateLogger<TicketCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<TicketListItem>> Handle(
        TicketsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<TicketListItem>("POST", "tickets", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling tickets list request: {@Query}", query);
            return WrappedListResponse<TicketListItem>.CreateFailureResponse(exception);
        }
    }

#if false
    public async Task<WrappedListResponse<TicketWithOrders>> Handle(
    TicketsWithOrdersListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<TicketWithOrders>("POST", "tickets/with-orders", payload: query, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling tickets list ticket: {@Query}", query);
            return WrappedListResponse<TicketWithOrders>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<Ticket?>> Handle(
        TicketQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<Ticket?>("GET", "tickets", query.TicketId, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling ticket details ticket: {@Ticket}", query);
            return WrappedResponse<Ticket?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<TicketWithOrders?>> Handle(
        TicketWithOrdersQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<TicketWithOrders?>("GET", "tickets/with-orders", query.TicketId, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling ticket details ticket: {@Ticket}", query);
            return WrappedResponse<TicketWithOrders?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<string?>> Handle(
        TicketTitleQuery query, CancellationToken token)
    {
        try
        {
            var response = await CallBackendReturnSingle<Ticket?>("GET", "tickets", query.TicketId, token: token);
            return response switch
            {
                { Success: false, Exception: not null } =>
                    WrappedResponse<string?>.CreateFailureResponse(response.Exception),
                { Success: false, Exception: null } =>
                    WrappedResponse<string?>.CreateFailureResponse(new SerializableException("Unknown Error")),
                { Success: true, Result: null } =>
                    WrappedResponse<string?>.CreateSuccessResponse(null),
                { Success: true, Result: not null } =>
                    WrappedResponse<string?>.CreateSuccessResponse(response.Result!.FullName)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling ticket details ticket: {@Ticket}", query);
            return WrappedResponse<string?>.CreateFailureResponse(exception);
        }
    } 
#endif
    #endregion
}