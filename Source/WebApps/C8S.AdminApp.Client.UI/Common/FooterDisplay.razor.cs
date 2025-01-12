using Microsoft.AspNetCore.Components;
using SC.Common.Interfaces;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Common;

public partial class FooterDisplay: BaseRazorComponent
{
    [Inject]
    public IDateTimeHelper DateTimeHelper { get; set; } = null!;
}