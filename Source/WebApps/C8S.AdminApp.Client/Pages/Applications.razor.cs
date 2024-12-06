using System.Diagnostics;
using C8S.AdminApp.Client.Components.Listers;
using C8S.AdminApp.Common.Interfaces;
using C8S.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using SC.Audit.Abstractions.Models;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public partial class Applications : BaseRazorPage, IDisposable
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
    public ILogger<Applications> Logger { get; set; } = default!;

    [Inject]
    public ICommunicationService CommunicationService { get; set; } = default!;

    private ApplicationsLister _applicationsLister = default!;
    private RadzenDropDown<IList<RequestStatus>> _statusDropDown = default!;
    private RadzenDropDown<string> _sortDropDown = default!;

    private string _selectedSort = "SubmittedOn DESC";
    private IList<RequestStatus> _selectedStatuses = [RequestStatus.Received];

    private int? _totalCount;
    

    protected override async Task OnInitializedAsync()
    {
        await CommunicationService.InitializeAsync();
        CommunicationService.DataChanged += HandleDataChanged;
        await base.OnInitializedAsync();
    }

    public void Dispose()
    {
        CommunicationService.DataChanged -= HandleDataChanged;
    }

    private void HandleDataChanged(object? sender, DataChangeEventArgs args)
    {
        var dataChange = args.DataChange;
        if (dataChange is { EntityName: "ApplicationDb", EntityState: EntityState.Added })
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
        var statuses = args as EnumerableQuery<RequestStatus> ??
                       throw new UnreachableException();
        return Task.CompletedTask;
    }

    private async Task ReloadLister()
    {
        Logger.LogInformation("Reloading lister.");
        await _applicationsLister.Reload().ConfigureAwait(false);
    }

    private record SortDropDownOption(string Display, string Value);
}