using System.Diagnostics;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using C8S.WordPress.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using Radzen;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Abstractions.Models;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Temp;

public sealed class WordPressUserCreatorCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IWordPressService wordPressService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    private readonly ILogger<WordPressUserCreatorCoordinator> _logger =
        loggerFactory.CreateLogger<WordPressUserCreatorCoordinator>();

    public string? Email { get; set; }

    public List<PersonListItem> Persons { get; set; } = [];
    public int? SelectedId { get; set; }
    public int TotalPersons { get; set; }

    public bool IsCreating { get; set; } = false;
    public bool IsLoading { get; set; } = false;

    public override void SetUp()
    {
        base.SetUp();
        LoadPersonsData(new LoadDataArgs() { Skip = 0, Top = 5 });
    }

    public async Task CreateWordPressUser()
    {
        IsCreating = true;
        await PerformComponentRefresh();

        try
        {
            if (!SelectedId.HasValue)
                throw new UnreachableException("CreateWordPressUser called without PersonId set.");

            var personResponse = await GetQueryResults<PersonDetailsQuery, DomainResponse<PersonDetails?>>(
                new PersonDetailsQuery() { PersonId = SelectedId.Value });
            var person = personResponse.Result ??
                throw new UnreachableException($"Could not find Person #{SelectedId.Value}");

            await wordPressService.CreateWordPressUserFromPerson(person);
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
        finally
        {
            IsCreating = false;
            await PerformComponentRefresh();
        }
    }

    public void LoadPersonsData(LoadDataArgs args) =>
        Task.Run(async () => await LoadPersonsList(args));

    private async Task LoadPersonsList(LoadDataArgs args)
    {
        if (!String.IsNullOrWhiteSpace(args.Filter) && args.Filter.Length < 3) return;
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var personsListResult = await GetQueryResults<PersonsListQuery, DomainResponse<PersonsListResults>>(
                new PersonsListQuery()
                {
                    StartIndex = args.Skip ?? 0,
                    Count = args.Top ?? 0,
                    Query = String.IsNullOrWhiteSpace(args.Filter) ? null : args.Filter,
                    SortDescription = "Email ASC"
                }) ?? throw new UnreachableException("GetQueryResults returned null");

            if (!personsListResult.Success || personsListResult.Result == null)
                throw personsListResult.Exception?.ToException() ??
                      new Exception("Error occurred loading persons");

            Persons = personsListResult.Result.Items;
            TotalPersons = personsListResult.Result.Total;

            _logger.LogDebug("Skip={Skip}, Top={Top}, Total={Total}",
                args.Skip, args.Top, TotalPersons);

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }
}