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
        ICQRSQueryHandler<PersonsListQuery, WrappedListResponse<PersonListItem>>,
        ICQRSQueryHandler<PersonDetailsQuery, WrappedResponse<PersonDetails?>>,
        ICQRSQueryHandler<PersonTitleQuery, WrappedResponse<string?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PersonCallbacks> _logger = loggerFactory.CreateLogger<PersonCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<PersonListItem>> Handle(
        PersonsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<PersonListItem>("POST", "persons", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling persons list person: {@Person}", query);
            return WrappedListResponse<PersonListItem>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<PersonDetails?>> Handle(
        PersonDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<PersonDetails?>("GET", "persons", query.PersonId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return WrappedResponse<PersonDetails?>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<string?>> Handle(
        PersonTitleQuery query, CancellationToken token)
    {
        try
        {
            var response = await CallBackendReturnSingle<PersonDetails?>("GET", "persons", query.PersonId, token:token);
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