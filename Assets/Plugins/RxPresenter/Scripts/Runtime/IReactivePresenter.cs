using System;
using UniRx;

namespace Boscohyun.RxPresenter
{
    public interface IReactivePresenter<out T> : IDisposable, IPresenter where T : IReactivePresenter<T>
    {
        IObservable<T> OnPresenterStateChange { get; }

        IObservable<T> OnShowAnimationBeginning { get; }

        IObservable<T> OnShowAnimationEnd { get; }

        IObservable<T> OnHideAnimationBeginning { get; }

        IObservable<T> OnHideAnimationEnd { get; }

        void Show(Action<T> callback);

        void Show(bool skipAnimation, Action<T> callback);
        
        IObservable<T> ShowAsObservable(bool skipAnimation = default);
        
        void Hide(Action callback);

        void Hide(bool skipAnimation, Action callback);
        
        IObservable<Unit> HideAsObservable(bool skipAnimation = default);
    }
}
