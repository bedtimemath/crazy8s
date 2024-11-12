using System.Diagnostics;
using C8S.AdminApp.Client.Components.Listers;
using C8S.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public partial class Applications: BaseRazorPage
{
    [Inject]
    public ILogger<Applications> Logger { get; set; } = default!;

    private ApplicationsLister _applicationsLister = default!;
    private string _sortDescription = "ApplicantEmail DESC";
    private int? _totalCount;

    private IList<ApplicationStatus> _selectedStatuses = 
        new [] { ApplicationStatus.Received };
    private RadzenDropDown<IList<ApplicationStatus>> _statusDropDown = default!;

    private async Task HandleStatusDropdownChange(object args)
    {
        var statuses = args as EnumerableQuery<ApplicationStatus> ??
                       throw new UnreachableException();

    }
}