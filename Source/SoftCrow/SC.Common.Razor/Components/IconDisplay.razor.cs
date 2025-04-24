using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace SC.Common.Razor.Components;

public partial class IconDisplay: BaseRazorComponent
{
    #region Constants & ReadOnlys
    private const string IconFormatString = "<i class=\"fa-{0} fa-{1}\"></i>";
    #endregion

    #region Component Parameters
    [Parameter]
    public string? IconGroup { get; set; }

    [Parameter]
    public string? IconString { get; set; }
    #endregion

    #region Component Properties
    public string IconHtml { get; private set; } = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        IconHtml =
            String.Format(IconFormatString, 
                String.IsNullOrEmpty(IconGroup) ? SoftCrowConstants.IconGroups.Regular : IconGroup, 
                String.IsNullOrEmpty(IconString) ? SoftCrowConstants.Icons.Empty : IconString);
    }
    #endregion
}