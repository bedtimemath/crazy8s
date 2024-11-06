using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;
using SC.Common;

namespace C8S.Common.Razor.Components;

public partial class IconDisplay: BaseRazorComponent
{
    #region Constants & ReadOnlys
    private const string IconFormatString = "<i class=\"fa-regular fa-{0}\"></i>";
    #endregion

    #region Component Parameters
    [Parameter]
    public string? Icon { get; set; }
    #endregion

    #region Component Properties
    public string IconHtml { get; private set; } = default!;
    #endregion

    #region Component LifeCycle
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        IconHtml =
            String.Format(IconFormatString, 
                String.IsNullOrEmpty(Icon) ? SharedConstants.Icons.Empty : Icon);
    }
    #endregion
}