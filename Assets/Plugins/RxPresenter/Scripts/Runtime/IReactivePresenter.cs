using System;
using Cysharp.Threading.Tasks;

namespace Boscohyun.RxPresenter
{
    public interface IReactivePresenter<T> : IDisposable, IPresenter where T : IPresenter
    {
        IObservable<T> OnPresenterStateChange { get; }

        IObservable<T> OnShowAnimationBeginning { get; }

        IObservable<T> OnShowAnimationEnd { get; }

        IObservable<T> OnHideAnimationBeginning { get; }

        IObservable<T> OnHideAnimationEnd { get; }

        void Show(Action<T> callback);

        void Show(bool skipAnimation, Action<T> callback);

        IObservable<T> ShowAsObservable(bool skipAnimation = default);

        UniTask<T> ShowAsync(bool skipAnimation = default);

        void Hide(Action<T> callback);

        void Hide(bool skipAnimation, Action<T> callback);

        IObservable<T> HideAsObservable(bool skipAnimation = default);
        
        UniTask<T> HideAsync(bool skipAnimation = default);
    }
}
