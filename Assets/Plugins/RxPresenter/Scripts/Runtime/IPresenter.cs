namespace Boscohyun.RxPresenter
{
    public interface IPresenter
    {
        HumbleReactivePresenter Humble { get; }
        
        PresenterState PresenterState { get; }

        void Show(bool skipAnimation = default);

        void Hide(bool skipAnimation = default);
    }
}
