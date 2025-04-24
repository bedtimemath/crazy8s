namespace SC.Common.Helpers.Base;

public interface ICoordinator: IDisposable
{
    Func<Task>? ComponentRefresh { get; set; }
    void SetUp();
    void TearDown();
    void TearDownUnmanaged();
}