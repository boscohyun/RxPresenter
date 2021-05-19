namespace Boscohyun.RxPresenter
{
    public interface IPresenter
    {
        PresenterState PresenterState { get; }

        void Show(bool skipAnimation = default);

        void Hide(bool skipAnimation = default);
    }
}
