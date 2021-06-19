using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Boscohyun.RxPresenter
{
    public class HumbleReactivePresenter : IReactivePresenter<HumbleReactivePresenter>
    {
        private readonly IView _view;

        private readonly ReactiveProperty<PresenterState> PresenterStateReactiveProperty =
            new ReactiveProperty<PresenterState>();

        private readonly Subject<HumbleReactivePresenter> ShowAnimationBeginningSubject = new Subject<HumbleReactivePresenter>();
        private readonly Subject<HumbleReactivePresenter> ShowAnimationEndSubject = new Subject<HumbleReactivePresenter>();
        private readonly Subject<HumbleReactivePresenter> HideAnimationBeginningSubject = new Subject<HumbleReactivePresenter>();
        private readonly Subject<HumbleReactivePresenter> HideAnimationEndSubject = new Subject<HumbleReactivePresenter>();

        public PresenterState PresenterState => PresenterStateReactiveProperty.Value;

        public IObservable<HumbleReactivePresenter> OnPresenterStateChange => PresenterStateReactiveProperty.Select(_ => this);
        public IObservable<HumbleReactivePresenter> OnShowAnimationBeginning => ShowAnimationBeginningSubject;
        public IObservable<HumbleReactivePresenter> OnShowAnimationEnd => ShowAnimationEndSubject;
        public IObservable<HumbleReactivePresenter> OnHideAnimationBeginning => HideAnimationBeginningSubject;
        public IObservable<HumbleReactivePresenter> OnHideAnimationEnd => HideAnimationEndSubject;

        public HumbleReactivePresenter(IView view)
        {
            _view = view ?? throw new ArgumentNullException($"{nameof(view)} is null.");
            if (_view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive)
            {
                _view.ViewAnimator.SetActive(false);
                _view.ViewAnimator.SetActive(false);
            }

            PresenterStateReactiveProperty.Value = _view.ActiveSelf
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

            return _view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(_view.ViewAnimator.AnimatorActiveDelayFrame).DoOnCompleted(ShowAnimationEnd)
                : observable.DoOnCompleted(ShowAnimationEnd);
        }

        protected virtual void ShowAnimationBeginning(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.ShowAnimation;
            _view.SetActive(true);

            if (_view.HasViewAnimator)
            {
                if (!_view.ViewAnimator.AnimatorAlwaysActive)
                {
                    _view.ViewAnimator.SetActive(true);
                }

                _view.ViewAnimator.PlayAnimation(ViewAnimatorState.Show, skip ? 1f : 0f);
            }

            ShowAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask ShowAnimationAsync()
        {
            if (_view.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    _view.ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Show);
                await UniTask.WaitWhile(() => _view.ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
            }
            else
            {
                await UniTask.CompletedTask;
            }
        }

        protected virtual void ShowAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Shown;

            if (_view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive)
            {
                _view.ViewAnimator.SetActive(false);
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

            return _view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(_view.ViewAnimator.AnimatorActiveDelayFrame).DoOnCompleted(HideAnimationEnd)
                : observable.DoOnCompleted(HideAnimationEnd);
        }

        protected virtual void HideAnimationBegin(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.HideAnimation;

            if (_view.HasViewAnimator)
            {
                if (!_view.ViewAnimator.AnimatorAlwaysActive)
                {
                    _view.ViewAnimator.SetActive(true);
                }

                _view.ViewAnimator.PlayAnimation(ViewAnimatorState.Hide, skip ? 1f : 0f);
            }

            HideAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask HideAnimationAsync()
        {
            if (_view.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    _view.ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Hide);
                await UniTask.WaitWhile(() => _view.ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
            }
            else
            {
                await UniTask.CompletedTask;
            }
        }

        protected virtual void HideAnimationEnd()
        {
            PresenterStateReactiveProperty.Value = PresenterState.Hidden;
            _view.SetActive(false);

            if (_view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive)
            {
                _view.ViewAnimator.SetActive(false);
            }

            HideAnimationEndSubject.OnNext(this);
        }

        #endregion
    }
}
