using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Common.Helpers.Base;

namespace SC.Common.Razor.Base;

public abstract class BaseCoordinatedDialog<TCoordinator>: BaseCoordinatedComponent<TCoordinator>
    where TCoordinator : class, ICoordinator
{
    #region Injected Variables
    //[Inject]
    //protected ILogger<BaseRazorDialog> BaseLogger { get; set; } = null!;

    [Inject]
    protected DialogService DialogService { get; set; } = null!;
    #endregion
}