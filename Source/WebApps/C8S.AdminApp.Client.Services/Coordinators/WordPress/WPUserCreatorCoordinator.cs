using System.Diagnostics;
using C8S.Domain.Features.Clubs.Models;
using C8S.Domain.Features.Clubs.Queries;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Notifications;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.WordPress;

public sealed class WPUserCreatorCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    private readonly ILogger<WPUserCreatorCoordinator> _logger = loggerFactory.CreateLogger<WPUserCreatorCoordinator>();

    public RadzenDropDownDataGrid<Person?> DataGrid { get; set; } = null!;

    public string? Email { get; set; }

    public IList<Person> Persons { get; set; } = [];
    public Person? SelectedPerson { get; set; }
    public int TotalPersons { get; set; }

    public bool IsCreating { get; set; } = false;
    public bool IsLoading { get; set; } = false;

    public override void SetUp()
    {
        base.SetUp();
        LoadPersonsData(new LoadDataArgs() { Skip = 0, Top = 5 });
    }

    public async Task SetSelectedPerson(Person person)
    {
        SelectedPerson = person;

        try
        {
            var response = await GetQueryResults<PersonWithOrdersQuery, WrappedResponse<PersonWithOrders?>>(
                               new PersonWithOrdersQuery() { PersonId = person.PersonId }) ??
                           throw new UnreachableException("GetQueryResults returned null");
            if (!response.Success)
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            var personWithOrders = response.Result!;
            var clubs = personWithOrders.ClubPersons.Select(cp => cp.Club).ToList();
            _logger.LogDebug("Person {Name} has {Count} clubs", personWithOrders.FullName, clubs.Count);
            foreach (var club in clubs)
            {
                _logger.LogDebug("{@Club}: {Count}", club, club.Orders.Count);
            }
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }

    public async Task CreateWordPressUser()
    {
        IsCreating = true;
        await PerformComponentRefresh();

        try
        {
            if (SelectedPerson == null)
                throw new UnreachableException("CreateWordPressUser called without person selected.");

            var response = await GetCommandResults<WPUserAddCommand, WrappedResponse<WPUserDetails>>(
                               new WPUserAddCommand()
                               {
                                   Email = SelectedPerson.Email,
                                   Name = SelectedPerson.FullName,
                                   FirstName = SelectedPerson.FirstName,
                                   LastName = SelectedPerson.LastName,
                                   Roles = ["coach"]
                               }) ??
                           throw new UnreachableException("GetCommandResults returned null");
            if (!response.Success || response.Result == null)
                throw response?.Exception?.ToException() ??
                      new Exception("Error adding WPUser");

            PubSubService.Publish(new WPUsersUpdated() { WPUser = response.Result });
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
        if (!string.IsNullOrWhiteSpace(args.Filter) && args.Filter.Length < 3) return;
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var response = await GetQueryResults<PersonsListQuery, WrappedListResponse<Person>>(
                new PersonsListQuery()
                {
                    StartIndex = args.Skip ?? 0,
                    Count = args.Top ?? 0,
                    Query = string.IsNullOrWhiteSpace(args.Filter) ? null : args.Filter,
                    SortDescription = "Email ASC"
                });
            if (response is { Success: false } or { Result: null } ) 
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            Persons = response.Result;
            TotalPersons = response.Total;

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }
}