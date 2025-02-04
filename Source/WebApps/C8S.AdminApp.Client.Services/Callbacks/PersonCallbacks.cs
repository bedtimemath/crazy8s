using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Common.Models;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class PersonCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<PersonsListQuery, DomainResponse<PersonsListResults>>,
        ICQRSQueryHandler<PersonDetailsQuery, DomainResponse<PersonDetails?>>,
        ICQRSQueryHandler<PersonTitleQuery, DomainResponse<string?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PersonCallbacks> _logger = loggerFactory.CreateLogger<PersonCallbacks>();
    #endregion

    #region Queries
    public async Task<DomainResponse<PersonsListResults>> Handle(
        PersonsListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<PersonsListResults>("POST", "persons", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling persons list person: {@Person}", query);
            return DomainResponse<PersonsListResults>.CreateFailureResponse(exception);
        }
    }

    public async Task<DomainResponse<PersonDetails?>> Handle(
        PersonDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<PersonDetails?>("GET", "persons", query.PersonId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return DomainResponse<PersonDetails?>.CreateFailureResponse(exception);
        }
    }

    public async Task<DomainResponse<string?>> Handle(
        PersonTitleQuery query, CancellationToken token)
    {
        try
        {
            var person = await CallBackendServer<PersonDetails?>("GET", "persons", query.PersonId, token:token);
            return person switch
            {
                { Success: false, Exception: not null } =>
                    DomainResponse<string?>.CreateFailureResponse(person.Exception),
                { Success: false, Exception: null } =>
                    DomainResponse<string?>.CreateFailureResponse(new SerializableException("Unknown Error")),
                { Success: true, Result: null } =>
                    DomainResponse<string?>.CreateSuccessResponse(null),
                { Success: true, Result: not null } =>
                    DomainResponse<string?>.CreateSuccessResponse(person.Result.FullName)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling person details person: {@Person}", query);
            return DomainResponse<string?>.CreateFailureResponse(exception);
        }
    }
    #endregion
}