using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Application;

public partial class ApplicationHeader: BaseRazorComponent
{
    [Parameter]
    public ApplicationDTO? Application { get; set; }
}