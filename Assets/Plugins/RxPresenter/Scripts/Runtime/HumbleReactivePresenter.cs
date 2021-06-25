using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Boscohyun.RxPresenter
{
    public class HumbleReactivePresenter : IReactivePresenter<HumbleReactivePresenter>
    {
        protected readonly IView View;

        protected readonly Action<bool> ShowAnimationBeginningDelegate;
        protected readonly Func<UniTask> ShowAnimationAsyncDelegate;
        protected readonly Action ShowAnimationEndDelegate;
        protected readonly Action<bool> HideAnimationBeginningDelegate;
        protected readonly Func<UniTask> HideAnimationAsyncDelegate;
        protected readonly Action HideAnimationEndDelegate;

        protected readonly ReactiveProperty<PresenterState> PresenterStateReactiveProperty =
            new ReactiveProperty<PresenterState>();

        protected readonly Subject<HumbleReactivePresenter> ShowAnimationBeginningSubject =
            new Subject<HumbleReactivePresenter>();

        protected readonly Subject<HumbleReactivePresenter> ShowAnimationEndSubject =
            new Subject<HumbleReactivePresenter>();

        protected readonly Subject<HumbleReactivePresenter> HideAnimationBeginningSubject =
            new Subject<HumbleReactivePresenter>();

        protected readonly Subject<HumbleReactivePresenter> HideAnimationEndSubject =
            new Subject<HumbleReactivePresenter>();

        public PresenterState PresenterState => PresenterStateReactiveProperty.Value;

        public IObservable<HumbleReactivePresenter> OnPresenterStateChange =>
            PresenterStateReactiveProperty.Select(_ => this);

        public IObservable<HumbleReactivePresenter> OnShowAnimationBeginning => ShowAnimationBeginningSubject;
        public IObservable<HumbleReactivePresenter> OnShowAnimationEnd => ShowAnimationEndSubject;
        public IObservable<HumbleReactivePresenter> OnHideAnimationBeginning => HideAnimationBeginningSubject;
        public IObservable<HumbleReactivePresenter> OnHideAnimationEnd => HideAnimationEndSubject;

        public HumbleReactivePresenter(IView view)
        {
            View = view ?? throw new ArgumentNullException($"{nameof(view)} is null.");
            if (View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive)
            {
                View.ViewAnimator.SetActive(false);
            }

            PresenterStateReactiveProperty.Value = View.ActiveSelf
                ? PresenterState.Shown
                : PresenterState.Hidden;
        }

        public HumbleReactivePresenter(
            IView view,
            Action<bool> showAnimationBeginning,
            Func<UniTask> showAnimationAsync,
            Action showAnimationEnd,
            Action<bool> hideAnimationBeginning,
            Func<UniTask> hideAnimationAsync,
            Action hideAnimationEnd) : this(view)
        {
            ShowAnimationBeginningDelegate = showAnimationBeginning;
            ShowAnimationAsyncDelegate = showAnimationAsync;
            ShowAnimationEndDelegate = showAnimationEnd;
            HideAnimationBeginningDelegate = hideAnimationBeginning;
            HideAnimationAsyncDelegate = hideAnimationAsync;
            HideAnimationEndDelegate = hideAnimationEnd;
        }

        public virtual void Dispose()
        {
            PresenterStateReactiveProperty.Dispose();
            ShowAnimationBeginningSubject.Dispose();
            ShowAnimationEndSubject.Dispose();
            HideAnimationBeginningSubject.Dispose();
            HideAnimationEndSubject.Dispose();
        }

        #region Show

        public virtual void Show(bool skipAnimation = default) => Show(skipAnimation, null);

        public virtual void Show(Action<HumbleReactivePresenter> callback) => Show(default, callback);

        public virtual void Show(bool skipAnimation, Action<HumbleReactivePresenter> callback) =>
            ShowAsObservable(skipAnimation).DoOnCompleted(() => callback?.Invoke(this)).Subscribe();

        public virtual IObservable<HumbleReactivePresenter> ShowAsObservable(bool skipAnimation = default)
        {
            ShowAnimationBeginning(skipAnimation);

            var observable = skipAnimation
                ? Observable
                    .Return(this)
                    .DoOnSubscribe(() => PresenterStateReactiveProperty.Value = PresenterState.Shown)
                : ShowAnimationAsync().ToObservable().Select(_ => this);

            return View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(View.ViewAnimator.AnimatorActiveDelayFrame).DoOnCompleted(ShowAnimationEnd)
                : observable.DoOnCompleted(ShowAnimationEnd);
        }

        public async UniTask<HumbleReactivePresenter> ShowAsync(bool skipAnimation = default) =>
            await ShowAsObservable(skipAnimation).ToTask();

        protected virtual void ShowAnimationBeginning(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.ShowAnimation;
            ShowAnimationBeginningDelegate?.Invoke(skip);
            ShowAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask ShowAnimationAsync()
        {
            if (ShowAnimationAsyncDelegate is null)
            {
                return;
            }

            await ShowAnimationAsyncDelegate.Invoke();
        }

        protected virtual void ShowAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Shown;
            ShowAnimationEndDelegate?.Invoke();
            ShowAnimationEndSubject.OnNext(this);
        }

        #endregion

        #region Hide

        public virtual void Hide(bool skipAnimation = default) => Hide(skipAnimation, null);

        public virtual void Hide(Action<HumbleReactivePresenter> callback) => Hide(default, callback);

        public virtual void Hide(bool skipAnimation, Action<HumbleReactivePresenter> callback) =>
            HideAsObservable(skipAnimation).DoOnCompleted(() => callback?.Invoke(this)).Subscribe();

        public virtual IObservable<HumbleReactivePresenter> HideAsObservable(bool skipAnimation = default)
        {
            HideAnimationBeginning(skipAnimation);

            var observable = skipAnimation
                ? Observable
                    .Return(this)
                    .DoOnSubscribe(() => PresenterStateReactiveProperty.Value = PresenterState.Hidden)
                : HideAnimationAsync().ToObservable().Select(_ => this);

            return View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(View.ViewAnimator.AnimatorActiveDelayFrame).DoOnCompleted(HideAnimationEnd)
                : observable.DoOnCompleted(HideAnimationEnd);
        }

        public async UniTask<HumbleReactivePresenter> HideAsync(bool skipAnimation = default) =>
            await HideAsObservable(skipAnimation).ToTask();

        protected virtual void HideAnimationBeginning(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.HideAnimation;
            HideAnimationBeginningDelegate?.Invoke(skip);
            HideAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask HideAnimationAsync()
        {
            if (HideAnimationAsyncDelegate is null)
            {
                return;
            }

            await HideAnimationAsyncDelegate.Invoke();
        }

        protected virtual void HideAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Hidden;
            HideAnimationEndDelegate?.Invoke();
            HideAnimationEndSubject.OnNext(this);
        }

        #endregion
    }
}
