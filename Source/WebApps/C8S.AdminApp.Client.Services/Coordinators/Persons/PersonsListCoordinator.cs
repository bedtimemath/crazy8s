using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Razor.Extensions;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;
using System.Diagnostics;

namespace C8S.AdminApp.Client.Services.Coordinators.Persons;

public sealed class PersonsListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IJSRuntime jsRuntime) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region Constants & ReadOnlys
    public const string ListerContainerId = "person-list-container";
    
    private const string InitialSort = "LastName DESC";
    #endregion

    #region ReadOnly Constructor Variables
    private readonly ILogger<PersonsListCoordinator> _logger = loggerFactory.CreateLogger<PersonsListCoordinator>();
    #endregion

    #region ReadOnly Variables
    public readonly IEnumerable<DropDownOption> SortDropDownOptions = [
        new( "Last Name (A-Z)", "LastName ASC" ),
        new( "Last Name (Z-A)", "LastName DESC" ),
        new( "Email (A-Z)", "Email ASC" ),
        new( "Email (Z-A)", "Email DESC" )
    ];
    #endregion

    #region Public Events
    public event EventHandler? FilterChanged;
    public void RaiseFilterChanged() => FilterChanged?.Invoke(this, EventArgs.Empty);

    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public string SelectedSort { get; set; } = InitialSort;
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
        Query = null;
        TotalCount = null;
        
        RaiseFilterChanged();
    }
    
    public async ValueTask<ItemsProviderResult<Person>>
        GetRows(ItemsProviderRequest person)
    {
        try
        {
            var response = await GetQueryResults<PersonsListQuery, WrappedListResponse<Person>>(
                    new PersonsListQuery()
                    {
                        StartIndex = person.StartIndex,
                        Count = person.Count,
                        Query = Query,
                        SortDescription = SelectedSort
                    });
            if (response is { Success: false } or { Result: null } ) 
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            var results = response.Result;
            TotalCount = results.Count;

            RaiseListUpdated();
            return new ItemsProviderResult<Person>(results, results.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting persons list");
            throw;
        }
    }
    #endregion

    #region Event Handlers
    public void HandleQueryValueChange() => RaiseFilterChanged();
    public void HandleSortDropdownChange() => RaiseFilterChanged();
    #endregion

    #region Internal Classes / Records
    public record DropDownOption(string Display, object? Value); 
    #endregion
}
