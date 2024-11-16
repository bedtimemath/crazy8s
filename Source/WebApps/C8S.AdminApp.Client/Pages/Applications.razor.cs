using System.Diagnostics;
using C8S.AdminApp.Client.Components.Listers;
using C8S.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public partial class Applications : BaseRazorPage
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

    private ApplicationsLister _applicationsLister = default!;
    private RadzenDropDown<IList<ApplicationStatus>> _statusDropDown = default!;
    private RadzenDropDown<string> _sortDropDown = default!;

    private string _selectedSort = "SubmittedOn DESC";
    private IList<ApplicationStatus> _selectedStatuses = [ApplicationStatus.Received];

    private int? _totalCount;

    private Task HandleSortDropdownChange(object args)
    {
        return Task.CompletedTask;
    }
    private Task HandleStatusDropdownChange(object args)
    {
        var statuses = args as EnumerableQuery<ApplicationStatus> ??
                       throw new UnreachableException();
        return Task.CompletedTask;
    }

    private record SortDropDownOption(string Display, string Value);
}