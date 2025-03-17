using C8S.Domain.Features.Clubs.Queries;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class PersonCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<PersonsListQuery, WrappedListResponse<Person>>,
        ICQRSQueryHandler<PersonQuery, WrappedResponse<Person?>>,
        ICQRSQueryHandler<PersonTitleQuery, WrappedResponse<string?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PersonCallbacks> _logger = loggerFactory.CreateLogger<PersonCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<Person>> Handle(
        PersonsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<Person>("POST", "persons", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling persons list person: {@Query}", query);
            return WrappedListResponse<Person>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedListResponse<PersonWithOrders>> Handle(
        PersonsWithOrdersListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<PersonWithOrders>("POST", "persons/with-orders", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling persons list person: {@Query}", query);
            return WrappedListResponse<PersonWithOrders>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<Person?>> Handle(
        PersonQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<Person?>("GET", "persons", query.PersonId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return WrappedResponse<Person?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<PersonWithOrders?>> Handle(
        PersonWithOrdersQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<PersonWithOrders?>("GET", "persons/with-orders", query.PersonId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return WrappedResponse<PersonWithOrders?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<string?>> Handle(
        PersonTitleQuery query, CancellationToken token)
    {
        try
        {
            var response = await CallBackendReturnSingle<Person?>("GET", "persons", query.PersonId, token:token);
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
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return WrappedResponse<string?>.CreateFailureResponse(exception);
        }
    }
    #endregion
}