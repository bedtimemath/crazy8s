using C8S.Domain.Features.Requests.Enums;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using SC.Common.Models;

namespace C8S.AdminApp.Client.Services.Controllers.Requests;

public sealed class RequestsListController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region Constants & ReadOnlys
    private const string InitialSort = "SubmittedOn DESC";
    private const RequestStatus InitialStatus = RequestStatus.Received;
    #endregion

    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestsListController> _logger = loggerFactory.CreateLogger<RequestsListController>();
    #endregion

    #region ReadOnly Variables
    public readonly IEnumerable<SortDropDownOption> SortDropDownOptions = [
        new( "Submitted (newest)", "SubmittedOn DESC" ),
        new( "Submitted (oldest)", "SubmittedOn ASC" ),
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

    public IList<RequestStatus> SelectedStatuses { get; set; } = [InitialStatus];

    public string? Query { get; set; } 

    public int? TotalCount { get; set; } 
    #endregion

    #region Public Methods
    public void ClearFilter()
    {
        SelectedSort = InitialSort;
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
            var backendResponse = await mediator.Send(new RequestsListQuery()
            {
                StartIndex = request.StartIndex,
                Count = request.Count,
                Query = Query,
                SortDescription = SelectedSort,
                Statuses = SelectedStatuses
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
    #endregion

    #region Internal Classes / Records
    public record SortDropDownOption(string Display, string Value); 
    #endregion
}
