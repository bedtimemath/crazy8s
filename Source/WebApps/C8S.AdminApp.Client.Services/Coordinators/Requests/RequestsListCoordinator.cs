using C8S.Domain.Features.Requests.Enums;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Models;
using SC.Common.Razor.Extensions;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;
using System.Diagnostics;

namespace C8S.AdminApp.Client.Services.Coordinators.Requests;

public sealed class RequestsListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IJSRuntime jsRuntime) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region Constants & ReadOnlys
    public const string ListerContainerId = "request-list-container";
    
    private const string InitialSort = "SubmittedOn DESC";
    private const RequestStatus InitialStatus = RequestStatus.Received;
    #endregion

    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestsListCoordinator> _logger = loggerFactory.CreateLogger<RequestsListCoordinator>();
    #endregion

    #region ReadOnly Variables
    public readonly IEnumerable<DropDownOption> SortDropDownOptions = [
        new( "Submitted (newest)", "SubmittedOn DESC" ),
        new( "Submitted (oldest)", "SubmittedOn ASC" ),
        new( "Coach Call (soonest)", "FullSlateAppointmentStartsOn ASC" ),
        new( "Coach Call (latest)", "FullSlateAppointmentStartsOn DESC" ),
        new( "Last Name (A-Z)", "PersonLastName ASC" ),
        new( "Last Name (Z-A)", "PersonLastName DESC" ),
        new( "Email (A-Z)", "PersonEmail ASC" ),
        new( "Email (Z-A)", "PersonEmail DESC" )
    ];
    public readonly IEnumerable<EnumLabel<RequestStatus>> StatusDropDownOptions = 
        EnumLabel<RequestStatus>.GetAllEnumLabels();
    #endregion

    #region Public Events
    public event EventHandler? FilterChanged;
    public void RaiseFilterChanged() => FilterChanged?.Invoke(this, EventArgs.Empty);

    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public string SelectedSort { get; set; } = InitialSort;
    public DateOnly? SelectedAfter { get; set; } = null;
    public DateOnly? SelectedBefore { get; set; } = null;
    public IList<RequestStatus> SelectedStatuses { get; set; } = [InitialStatus];

    public string? Query { get; set; } 

    public int? TotalCount { get; set; } 
    #endregion

    #region Public Methods
    public async Task ScrollListToTop()
    {
        await jsRuntime.ScrollToTop(ListerContainerId);
    }
    
    public void ClearFilter()
    {
        SelectedSort = InitialSort;
        SelectedAfter = null;
        SelectedBefore = null;
        SelectedStatuses = [InitialStatus];
        Query = null;
        TotalCount = null;
        
        RaiseFilterChanged();
    }
    
    public async ValueTask<ItemsProviderResult<RequestListItem>>
        GetRows(ItemsProviderRequest request)
    {
        try
        {
            var hasCoachCall = SelectedSort.StartsWith("FullSlateAppointmentStartsOn") ? true : (bool?)null;
            var response = await GetQueryResults<RequestsListQuery, WrappedListResponse<RequestListItem>>(
                    new RequestsListQuery()
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
            TotalCount = results.Count;

            RaiseListUpdated();
            return new ItemsProviderResult<RequestListItem>(results, results.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting requests list");
            throw;
        }
    }
    #endregion

    #region Event Handlers
    public void HandleQueryValueChange() => RaiseFilterChanged();
    public void HandleSortDropdownChange() => RaiseFilterChanged();
    public void HandleStatusDropdownChange() => RaiseFilterChanged();
    public void HandleAfterDatePickerChange() => RaiseFilterChanged();
    public void HandleBeforeDatePickerChange() => RaiseFilterChanged();
    #endregion

    #region Internal Classes / Records
    public record DropDownOption(string Display, object? Value); 
    #endregion
}
