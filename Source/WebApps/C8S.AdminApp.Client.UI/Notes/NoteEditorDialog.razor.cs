using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.UI.Notes;

public partial class NoteEditorDialog: BaseRazorDialog
{
    #region Public Properties
    private string? _htmlContent;
    #endregion
    
    #region Component Parameters
    [Parameter]
    public string? InitialContent { get; set; }
    #endregion

    #region Dialog LifeCycle
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _htmlContent = InitialContent;
    }
    #endregion
}