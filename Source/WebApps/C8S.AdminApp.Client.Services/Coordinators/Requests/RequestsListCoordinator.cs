using C8S.Domain.Features.Requests.Enums;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Interfaces;
using SC.Common.Models;
using SC.Common.Razor.Extensions;

namespace C8S.AdminApp.Client.Services.Coordinators.Requests;

public sealed class RequestsListCoordinator(
    ILoggerFactory loggerFactory,
    IJSRuntime jsRuntime,
    IMediator mediator,
    IDateTimeHelper dateTimeHelper)
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
    public readonly IEnumerable<DropDownOption> SinceDropDownOptions = [
        new( "Anytime", null ),
        new( "In the last day", DateOnly.FromDateTime(dateTimeHelper.Now.DateTime).AddDays(-1)  ),
        new( "In the last week", DateOnly.FromDateTime(dateTimeHelper.Now.DateTime).AddDays(-7)  ),
        new( "In the last month", DateOnly.FromDateTime(dateTimeHelper.Now.DateTime).AddMonths(-1)  ),
        new( "In the last year", DateOnly.FromDateTime(dateTimeHelper.Now.DateTime).AddYears(-1)  )
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
            var backendResponse = await mediator.Send(new RequestsListQuery()
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
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            var results = backendResponse.Result!;
            TotalCount = results.Total;

            RaiseListUpdated();
            return new ItemsProviderResult<RequestListItem>(results.Items, results.Total);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Getting Rows");
            return default;
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
