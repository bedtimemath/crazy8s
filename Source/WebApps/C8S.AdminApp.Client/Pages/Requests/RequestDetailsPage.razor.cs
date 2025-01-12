using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages.Requests;

public partial class RequestDetailsPage: BaseRazorPage
{
    [Parameter]
    public int RequestId { get; set; }
}