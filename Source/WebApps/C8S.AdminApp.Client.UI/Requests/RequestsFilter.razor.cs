using C8S.AdminApp.Client.Services.Controllers.Requests;
using C8S.AdminApp.Client.UI.Base;
using C8S.Domain.Features.Requests.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radzen.Blazor;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Client.UI.Requests;

public sealed partial class RequestsFilter: BaseClientComponent, IDisposable
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
    public ILogger<RequestsFilter> Logger { get; set; } = null!;
    
    [Parameter]
    public RequestsListController Controller { get; set; } = null!;

    private RadzenDropDown<IList<RequestStatus>> _statusDropDown = null!;
    private RadzenDropDown<string> _sortDropDown = null!;

    private string _selectedSort = "SubmittedOn DESC";
    private IList<RequestStatus> _selectedStatuses = [RequestStatus.Received];

    private int? _totalCount;
    
    protected override async Task OnInitializedAsync()
    {
        //await CommunicationService.InitializeAsync();
        //CommunicationService.DataChanged += HandleDataChanged;
        await base.OnInitializedAsync();
    }

    public void Dispose()
    {
        if (CommunicationService != null)
            CommunicationService.DataChanged -= HandleDataChanged;
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

    private Task ReloadLister()
    {
        Logger.LogInformation("Reloading lister.");
        return Task.CompletedTask;
    }

    private record SortDropDownOption(string Display, string Value);
}
