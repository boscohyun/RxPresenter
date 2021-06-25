#if RXPRESENTER_DOTWEEN_SUPPORT
using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Boscohyun.RxPresenter.External.DOTween
{
    public abstract class DOTweenReactivePresenter<T> : DOTweenPresenter, IReactivePresenter<T>
        where T : DOTweenReactivePresenter<T>
    {
        public IObservable<T> OnPresenterStateChange => Humble.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationBeginning => Humble.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationEnd => Humble.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationBeginning => Humble.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationEnd => Humble.OnPresenterStateChange.Select(_ => (T) this);

        public void Show(Action<T> callback) => Humble.Show(_ => callback?.Invoke((T) this));

        public void Show(bool skipAnimation, Action<T> callback) =>
            Humble.Show(skipAnimation, _ => callback?.Invoke((T) this));

        public IObservable<T> ShowAsObservable(bool skipAnimation = default) =>
            Humble.ShowAsObservable(skipAnimation).Select(_ => (T) this);

        public async UniTask<T> ShowAsync(bool skipAnimation = default) =>
            await ShowAsObservable(skipAnimation).ToTask();

        public void Hide(Action<T> callback) => Humble.Hide(_ => callback?.Invoke((T) this));

        public void Hide(bool skipAnimation, Action<T> callback) =>
            Humble.Hide(skipAnimation, _ => callback?.Invoke((T) this));

        public IObservable<T> HideAsObservable(bool skipAnimation = default) =>
            Humble.HideAsObservable(skipAnimation).Select(_ => (T) this);
        
        public async UniTask<T> HideAsync(bool skipAnimation = default) =>
            await HideAsObservable(skipAnimation).ToTask();
    }
}
#endif
