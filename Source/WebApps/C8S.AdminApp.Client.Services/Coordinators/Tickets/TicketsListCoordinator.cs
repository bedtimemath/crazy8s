using C8S.AdminApp.Client.Services.Coordinators.Base;
using C8S.Domain.Enums;
using C8S.Domain.Features.Tickets.Models;
using C8S.Domain.Features.Tickets.Queries;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using System.Diagnostics;
using static C8S.AdminApp.Client.Services.Coordinators.Persons.PersonsListCoordinator;

namespace C8S.AdminApp.Client.Services.Coordinators.Tickets;

public class TicketsListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IJSRuntime jsRuntime) : BaseListCoordinator<TicketListItem>(loggerFactory, pubSubService, cqrsService, jsRuntime)
{
    #region Constants & ReadOnlys
    private const string InitialSort = "SubmittedOn DESC";
    private const TicketStatus InitialStatus = TicketStatus.Pending;
    #endregion

    #region ReadOnly Constructor Variables
    private readonly ILogger<TicketsListCoordinator> _logger = loggerFactory.CreateLogger<TicketsListCoordinator>();
    #endregion

    #region ReadOnly Variables
    public readonly IEnumerable<DropDownOption> SortDropDownOptions = [
        new( "Submitted (newest)", "SubmittedOn DESC" ),
        new( "Submitted (oldest)", "SubmittedOn ASC" ),
        new( "Coach Call (soonest)", "AppointmentStartsOn ASC" ),
        new( "Coach Call (latest)", "AppointmentStartsOn DESC" ),
        new( "Last Name (A-Z)", "LastName ASC" ),
        new( "Last Name (Z-A)", "LastName DESC" ),
        new( "Email (A-Z)", "Email ASC" ),
        new( "Email (Z-A)", "Email DESC" )
    ];
    public readonly IEnumerable<EnumLabel<TicketStatus>> StatusDropDownOptions = 
        EnumLabel<TicketStatus>.GetAllEnumLabels();
    #endregion

    #region Override Properties
    public override string ListContainerId => "tickets-list";
    #endregion

    #region Public Properties
    public string SelectedSort { get; set; } = InitialSort;
    public DateOnly? SelectedAfter { get; set; } = null;
    public DateOnly? SelectedBefore { get; set; } = null;
    public IList<TicketStatus> SelectedStatuses { get; set; } = [InitialStatus];

    public string? Query { get; set; } 

    public int? TotalCount { get; set; } 
    #endregion

    #region Override Methods
    public override async ValueTask<ItemsProviderResult<TicketListItem>>
        GetRows(ItemsProviderRequest request)
    {
        var itemsProviderResult = new ItemsProviderResult<TicketListItem>();

        try
        {
            var hasCoachCall = SelectedSort.StartsWith("AppointmentStartsOn") ? true : (bool?)null;
            var response = await GetQueryResults<TicketsListQuery, WrappedListResponse<TicketListItem>>(
                new TicketsListQuery()
                {
                    StartIndex = request.StartIndex,
                    Count = request.Count,
                    Query = Query,
                    SortDescription = SelectedSort,
                    SubmittedAfter = SelectedAfter,
                    SubmittedBefore = SelectedBefore,
                    Statuses = SelectedStatuses,
                    HasCoachCall = hasCoachCall
                });
            if (response is { Success: false } or { Result: null } ) 
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");
            
            var results = response.Result;
            TotalCount = response.Total;
            
            RaiseListUpdated();
            itemsProviderResult = new ItemsProviderResult<TicketListItem>(results, TotalCount ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rows");
            PubSubService.PublishException(ex);
        }
        
        return itemsProviderResult;
    }
    #endregion

}