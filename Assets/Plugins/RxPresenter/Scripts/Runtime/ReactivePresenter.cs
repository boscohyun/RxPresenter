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
            PostConstruct(_view.ActiveSelf
                ? PresenterState.Shown
                : PresenterState.Hidden);
        }

        public ReactivePresenter(IView view, PresenterState state)
        {
            _view = view ?? throw new ArgumentNullException($"{nameof(view)} is null.");
            PostConstruct(state);
        }

        private void PostConstruct(PresenterState state)
        {
            if (_view.HasViewAnimator && !_view.ViewAnimator.AnimatorAlwaysActive)
            {
                _view.ViewAnimator.SetActive(false);
            }

            PresenterStateReactiveProperty.Value = state;
        }

        public virtual void Dispose()
        {
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
            if (PresenterStateReactiveProperty.Value != PresenterState.Hidden)
            {
                return Observable.Return(this);
            }

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
                    _view.ViewAnimator.SetActive(true);
                }

                _view.ViewAnimator.PlayAnimation(ViewAnimationName.Show, skip ? 1f : 0f);
            }

            ShowAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask ShowAnimationAsync()
        {
            if (_view.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    _view.ViewAnimator.CurrentAnimationName != ViewAnimationName.Show);
                await UniTask.WaitWhile(() => _view.ViewAnimator.CurrentAnimationNormalizedTime < 1f);
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

        public virtual void Hide(Action callback) => Hide(default, callback);

        public virtual void Hide(bool skipAnimation, Action callback) =>
            HideAsObservable(skipAnimation).Subscribe(_ => callback?.Invoke());

        public virtual IObservable<Unit> HideAsObservable(bool skipAnimation = default)
        {
            if (PresenterStateReactiveProperty.Value != PresenterState.Shown)
            {
                return Observable.Empty<Unit>();
            }

            HideAnimationBegin(skipAnimation);

            var observable = skipAnimation
                ? Observable.Return(Unit.Default)
                : HideAnimationAsync().ToObservable().Select(_ => Unit.Default);

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
                    _view.ViewAnimator.SetActive(true);
                }

                _view.ViewAnimator.PlayAnimation(ViewAnimationName.Hide, skip ? 1f : 0f);
            }

            HideAnimationBeginningSubject.OnNext(this);
        }

        protected virtual async UniTask HideAnimationAsync()
        {
            if (_view.HasViewAnimator)
            {
                await UniTask.WaitWhile(() =>
                    _view.ViewAnimator.CurrentAnimationName != ViewAnimationName.Hide);
                await UniTask.WaitWhile(() => _view.ViewAnimator.CurrentAnimationNormalizedTime < 1f);
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
