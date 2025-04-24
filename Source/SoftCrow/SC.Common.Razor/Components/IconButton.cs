using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace SC.Common.Razor.Components;

public class IconButton: RadzenButton
{
    [Parameter]
    public string? IconGroup { get; set; }

    [Parameter]
    public string? IconString
    {
        get => SoftCrowRegex.ReadNameFromFontAwesomeIcon(this.Icon);
        set
        {
            var group = IconGroup ?? SoftCrowConstants.IconGroups.Regular;
            this.Icon = $"<i class=\"fa-{group} fa-{value}\"></i>";
        }
    }
}