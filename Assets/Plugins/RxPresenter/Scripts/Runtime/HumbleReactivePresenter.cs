using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Boscohyun.RxPresenter
{
    public class HumbleReactivePresenter : IReactivePresenter<HumbleReactivePresenter>
    {
        protected readonly IView View;

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

        protected virtual void ShowAnimationBeginning(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.ShowAnimation;
            View.SetActive(true);

            if (View.HasViewAnimator)
            {
                if (!View.ViewAnimator.AnimatorAlwaysActive)
                {
                    View.ViewAnimator.SetActive(true);
                }

                View.ViewAnimator.PlayAnimation(ViewAnimatorState.Show, skip ? 1f : 0f);
            }

            ShowAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask ShowAnimationAsync()
        {
            if (View.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    View.ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Show);
                await UniTask.WaitWhile(() => View.ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
            }
            else
            {
                await UniTask.CompletedTask;
            }
        }

        protected virtual void ShowAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Shown;

            if (View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive)
            {
                View.ViewAnimator.SetActive(false);
            }

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
            HideAnimationBegin(skipAnimation);

            var observable = skipAnimation
                ? Observable
                    .Return(this)
                    .DoOnSubscribe(() => PresenterStateReactiveProperty.Value = PresenterState.Hidden)
                : HideAnimationAsync().ToObservable().Select(_ => this);

            return View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(View.ViewAnimator.AnimatorActiveDelayFrame).DoOnCompleted(HideAnimationEnd)
                : observable.DoOnCompleted(HideAnimationEnd);
        }

        protected virtual void HideAnimationBegin(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.HideAnimation;

            if (View.HasViewAnimator)
            {
                if (!View.ViewAnimator.AnimatorAlwaysActive)
                {
                    View.ViewAnimator.SetActive(true);
                }

                View.ViewAnimator.PlayAnimation(ViewAnimatorState.Hide, skip ? 1f : 0f);
            }

            HideAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask HideAnimationAsync()
        {
            if (View.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    View.ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Hide);
                await UniTask.WaitWhile(() => View.ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
            }
            else
            {
                await UniTask.CompletedTask;
            }
        }

        protected virtual void HideAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Hidden;
            View.SetActive(false);

            if (View.HasViewAnimator && !View.ViewAnimator.AnimatorAlwaysActive)
            {
                View.ViewAnimator.SetActive(false);
            }

            HideAnimationEndSubject.OnNext(this);
        }

        #endregion
    }
}
