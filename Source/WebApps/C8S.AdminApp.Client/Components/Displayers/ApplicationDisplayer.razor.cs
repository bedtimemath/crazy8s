using C8S.Domain.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.Components.Displayers;

public partial class ApplicationDisplayer: BaseRazorComponent
{
    [Parameter]
    public ApplicationBase? Application { get; set; } = null;
}