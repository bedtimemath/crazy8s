using C8S.AdminApp.Client.UI.Requests;
using C8S.AdminApp.Common.Interfaces;
using C8S.Domain.Features.Requests.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using SC.Audit.Abstractions.Models;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public partial class RequestsListPage : BaseRazorPage, IDisposable
{
    private readonly IList<SortDropDownOption> _sortDropDownOptions = [
        new( "Submitted (newest)", "SubmittedOn DESC" ),
        new( "Submitted (oldest)", "SubmittedOn ASC" ),
        new( "Last Name (A-Z)", "ApplicantLastName ASC" ),
        new( "Last Name (Z-A)", "ApplicantLastName ASC" ),
        new( "Email (A-Z)", "ApplicantEmail ASC" ),
        new( "Email (Z-A)", "ApplicantEmail DESC" )
    ];

    [Inject]
    public ILogger<RequestsListPage> Logger { get; set; } = null!;

    [Inject]
    public ICommunicationService CommunicationService { get; set; } = null!;

    private RequestsListLister _requestsListLister = null!;
    private RadzenDropDown<IList<RequestStatus>> _statusDropDown = null!;
    private RadzenDropDown<string> _sortDropDown = null!;

    private string _selectedSort = "SubmittedOn DESC";
    private IList<RequestStatus> _selectedStatuses = [RequestStatus.Received];

    private int? _totalCount;
    

    protected override async Task OnInitializedAsync()
    {
        await CommunicationService.InitializeAsync();
        CommunicationService.DataChanged += HandleDataChanged;
        await base.OnInitializedAsync();
    }

    /* Key Points:
       
       1. Dispose(bool disposing):
          - This method is used to differentiate between disposing managed resources (disposing == true) and 
            unmanaged resources (disposing == false).
          - Managed resources are disposed only when disposing is true.
       
       2. GC.SuppressFinalize(this):
          - This prevents the garbage collector from calling the finalizer (~MyClass) since the resources have 
            already been cleaned up.
       
       3. Finalizer (~MyClass):
          - The finalizer is a safety net to clean up unmanaged resources if Dispose is not called explicitly.
       
       This pattern ensures proper resource management and avoids unnecessary finalization overhead.
     */
    private bool _disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent finalizer from being called
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources here
                CommunicationService.DataChanged -= HandleDataChanged;
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
    // Finalizer (destructor)
    ~RequestsListPage()
    {
        Dispose(false);
    }

    private void HandleDataChanged(object? sender, DataChangedEventArgs args)
    {
        var dataChange = args.DataChange;
        if (dataChange is { EntityName: "RequestDb", EntityState: EntityState.Added })
        {
            Task.Run(async () => await ReloadLister().ConfigureAwait(false));
        }
    }

    private Task HandleSortDropdownChange(object args)
    {
        return Task.CompletedTask;
    }
    private Task HandleStatusDropdownChange(object args)
    {
        //var statuses = args as EnumerableQuery<RequestStatus> ??
        //               throw new UnreachableException();
        return Task.CompletedTask;
    }

    private async Task ReloadLister()
    {
        Logger.LogInformation("Reloading lister.");
        await _requestsListLister.Reload().ConfigureAwait(false);
    }

    private record SortDropDownOption(string Display, string Value);
}