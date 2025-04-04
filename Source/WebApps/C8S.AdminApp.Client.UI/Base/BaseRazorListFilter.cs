using C8S.AdminApp.Client.Services.Coordinators.Base;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Base;

public abstract class BaseRazorListFilter<TCoordinator, TListItem> : BaseRazorComponent
    where TCoordinator: BaseListCoordinator<TListItem> 
    where TListItem: class, new()
{
    #region Component Parameters
    public abstract TCoordinator Coordinator { get; set; }
    #endregion
}