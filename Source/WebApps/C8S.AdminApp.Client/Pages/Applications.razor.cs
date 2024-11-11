using C8S.AdminApp.Client.Components.Listers;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

public partial class Applications: BaseRazorPage
{
    private ApplicationsLister _applicationsLister = default!;
    private string _sortDescription = "ApplicantEmail DESC";
    private int? _totalCount;
}