using System.Diagnostics;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using C8S.Domain.Features.Skus.Enums;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Extensions;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
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

    public RadzenDropDownDataGrid<PersonWithOrders?> DataGrid { get; set; } = null!;

    public IList<PersonWithOrders> Persons { get; set; } = [];
    public PersonWithOrders? SelectedPerson { get; set; }
    public int TotalPersons { get; set; }

    public IEnumerable<SkuYearOption> SkuYears { get; private set; } = [];

    public bool IsCreating { get; set; } = false;
    public bool IsLoading { get; set; } = false;

    public override void SetUp()
    {
        base.SetUp();
        LoadPersonsData(new LoadDataArgs() { Skip = 0, Top = 5 });
    }

    public async Task SetSkuYears(IEnumerable<SkuYearOption> skuYears)
    {
        SkuYears = skuYears;
        await DataGrid.Reload();
    }

    public async Task CreateWordPressUser()
    {
        IsCreating = true;
        await PerformComponentRefresh();

        try
        {
            if (SelectedPerson == null)
                throw new UnreachableException("CreateWordPressUser called without person selected.");

            var rolesResponse = await GetQueryResults<WPRolesListQuery, WrappedListResponse<WPRoleDetails>>(
                new WPRolesListQuery()) ?? throw new UnreachableException("GetQueryResults returned null");
            if (!rolesResponse.Success || rolesResponse.Result == null)
                throw rolesResponse?.Exception?.ToException() ??
                      new Exception("Error getting WordPress roles");

            var roleSlugs = rolesResponse.Result.Select(r => r.Slug);
            var skuSlugs = SelectedPerson.ClubPersons
                .Select(cp => cp.Club)
                .SelectMany(c => c.Orders)
                .SelectMany(o => o.OrderSkus)
                .Select(os => os.Sku.ClubKey.ToSlug());

            List<string> userRoles = ["coach"];
            userRoles.AddRange(roleSlugs.Intersect(skuSlugs));

            var addResponse = await GetCommandResults<WPUserAddCommand, WrappedResponse<WPUserDetails>>(
                               new WPUserAddCommand()
                               {
                                   Email = SelectedPerson.Email,
                                   Name = SelectedPerson.FullName,
                                   FirstName = SelectedPerson.FirstName,
                                   LastName = SelectedPerson.LastName,
                                   Roles = userRoles
                               }) ??
                           throw new UnreachableException("GetCommandResults returned null");
            if (!addResponse.Success || addResponse.Result == null)
                throw addResponse?.Exception?.ToException() ??
                      new Exception("Error adding WPUser");

            // clear the dropdown
            SelectedPerson = null;
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
        _logger.LogDebug("Args: {@Args}", args);

        if (!string.IsNullOrWhiteSpace(args.Filter) && args.Filter.Length < 3) return;
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var response = await GetQueryResults<PersonsWithOrdersListQuery, WrappedListResponse<PersonWithOrders>>(
                new PersonsWithOrdersListQuery()
                {
                    StartIndex = args.Skip ?? 0,
                    Count = args.Top ?? 0,
                    Query = string.IsNullOrWhiteSpace(args.Filter) ? null : args.Filter,
                    SkuYears = SkuYears.ToList(),
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