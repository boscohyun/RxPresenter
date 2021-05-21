using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Boscohyun.RxPresenter
{
    public class ReactivePresenter : IReactivePresenter<ReactivePresenter>
    {
        private readonly IView _view;

        protected readonly ReactiveProperty<PresenterState> PresenterStateReactiveProperty =
            new ReactiveProperty<PresenterState>();

        protected readonly Subject<ReactivePresenter> ShowAnimationBeginningSubject = new Subject<ReactivePresenter>();
        protected readonly Subject<ReactivePresenter> ShowAnimationEndSubject = new Subject<ReactivePresenter>();
        protected readonly Subject<ReactivePresenter> HideAnimationBeginningSubject = new Subject<ReactivePresenter>();
        protected readonly Subject<ReactivePresenter> HideAnimationEndSubject = new Subject<ReactivePresenter>();

        public PresenterState PresenterState => PresenterStateReactiveProperty.Value;

        public IObservable<ReactivePresenter> OnPresenterStateChange => PresenterStateReactiveProperty.Select(_ => this);
        public IObservable<ReactivePresenter> OnShowAnimationBeginning => ShowAnimationBeginningSubject;
        public IObservable<ReactivePresenter> OnShowAnimationEnd => ShowAnimationEndSubject;
        public IObservable<ReactivePresenter> OnHideAnimationBeginning => HideAnimationBeginningSubject;
        public IObservable<ReactivePresenter> OnHideAnimationEnd => HideAnimationEndSubject;

        public ReactivePresenter(IView view)
        {
            _view = view ?? throw new ArgumentNullException($"{nameof(view)} is null.");
            if (_view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive)
            {
                _view.ViewAnimator.SetActive(ViewAnimatorState.Show, false);
                _view.ViewAnimator.SetActive(ViewAnimatorState.Hide, false);
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

        public virtual void Show(Action<ReactivePresenter> callback) => Show(default, callback);

        public virtual void Show(bool skipAnimation, Action<ReactivePresenter> callback) =>
            ShowAsObservable(skipAnimation).Subscribe(_ => callback?.Invoke(this));

        public virtual IObservable<ReactivePresenter> ShowAsObservable(bool skipAnimation = default)
        {
            ShowAnimationBeginning(skipAnimation);

            var observable = skipAnimation
                ? Observable.Return(this)
                : ShowAnimationAsync().ToObservable().Select(_ => this);

            return _view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(1).DoOnCompleted(ShowAnimationEnd)
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
                    _view.ViewAnimator.SetActive(ViewAnimatorState.Show, true);
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
                _view.ViewAnimator.SetActive(ViewAnimatorState.Show, false);
            }

            ShowAnimationEndSubject.OnNext(this);
        }

        #endregion

        #region Hide

        public virtual void Hide(bool skipAnimation = default) => Hide(skipAnimation, null);

        public virtual void Hide(Action<ReactivePresenter> callback) => Hide(default, callback);

        public virtual void Hide(bool skipAnimation, Action<ReactivePresenter> callback) =>
            HideAsObservable(skipAnimation).Subscribe(_ => callback?.Invoke(this));

        public virtual IObservable<ReactivePresenter> HideAsObservable(bool skipAnimation = default)
        {
            HideAnimationBegin(skipAnimation);

            var observable = skipAnimation
                ? Observable.Return(this)
                : HideAnimationAsync().ToObservable().Select(_ => this);

            return _view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive
                ? observable.DelayFrame(1).DoOnCompleted(HideAnimationEnd)
                : observable.DoOnCompleted(HideAnimationEnd);
        }

        protected virtual void HideAnimationBegin(bool skip = default)
        {
            PresenterStateReactiveProperty.Value = PresenterState.HideAnimation;

            if (_view.HasViewAnimator)
            {
                if (!_view.ViewAnimator.AnimatorAlwaysActive)
                {
                    _view.ViewAnimator.SetActive(ViewAnimatorState.Hide, true);
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
                _view.ViewAnimator.SetActive(ViewAnimatorState.Hide, false);
            }

            HideAnimationEndSubject.OnNext(this);
        }

        #endregion
    }
}
