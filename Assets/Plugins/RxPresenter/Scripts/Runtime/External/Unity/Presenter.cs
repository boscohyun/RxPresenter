#if RXPRESENTER_UNITY_SUPPORT
using System;
using UniRx;
using UnityEngine;

namespace Boscohyun.RxPresenter.External.Unity
{
    [DisallowMultipleComponent]
    public class Presenter<T> : MonoBehaviour, IReactivePresenter<T>, IView, IViewAnimator
        where T : Presenter<T>
    {
        protected static readonly int AnimatorHashShow = Animator.StringToHash("Show");
        protected static readonly int AnimatorHashHide = Animator.StringToHash("Hide");

        private ReactivePresenter _reactivePresenter;

        [SerializeField] private Animator animator;
        [SerializeField] private bool animatorAlwaysEnabled;
        [SerializeField] private bool showAtAwake;

        #region IView

        bool IView.ActiveSelf => gameObject.activeSelf;

        bool IView.HasViewAnimator => animator;

        IViewAnimator IView.ViewAnimator => this;

        void IView.SetActive(bool active) => gameObject.SetActive(active);

        #endregion

        #region IViewAnimator

        bool IViewAnimator.AnimatorAlwaysActive => animatorAlwaysEnabled;

        ViewAnimationName IViewAnimator.CurrentAnimationName =>
            animator.GetCurrentAnimatorStateInfo(default).shortNameHash == AnimatorHashHide
                ? ViewAnimationName.Hide
                : ViewAnimationName.Show;

        float IViewAnimator.CurrentAnimationNormalizedTime =>
            animator.GetCurrentAnimatorStateInfo(default).normalizedTime;
        
        void IViewAnimator.PlayAnimation(ViewAnimationName viewAnimationName, float normalizedTime) =>
            animator.Play(
                viewAnimationName == ViewAnimationName.Hide
                    ? AnimatorHashHide
                    : AnimatorHashShow,
                default,
                normalizedTime);

        void IViewAnimator.SetActive(bool active) => animator.enabled = active;

        #endregion

        public PresenterState PresenterState => _reactivePresenter.PresenterState;
        
        public IObservable<T> OnPresenterStateChange => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationBeginning => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationEnd => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationBeginning => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationEnd => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);

        protected virtual void Awake()
        {
            if (showAtAwake)
            {
                _reactivePresenter = new ReactivePresenter(this, PresenterState.Hidden);
                _reactivePresenter.Show();
            }
            else
            {
                _reactivePresenter = new ReactivePresenter(this);
            }
        }

        protected virtual void OnDestroy()
        {
            ((IDisposable) this).Dispose();
        }

        void IDisposable.Dispose()
        {
            _reactivePresenter.Dispose();
        }

        public void Show(bool skipAnimation = default) => _reactivePresenter.Show(skipAnimation);

        public void Show(Action<T> callback) => _reactivePresenter.Show(_ => callback?.Invoke((T) this));

        public void Show(bool skipAnimation, Action<T> callback) =>
            _reactivePresenter.Show(skipAnimation, _ => callback?.Invoke((T) this));

        public IObservable<T> ShowAsObservable(bool skipAnimation = default) =>
            _reactivePresenter.ShowAsObservable(skipAnimation).Select(_ => (T) this);

        public void Hide(bool skipAnimation = default) => _reactivePresenter.Hide(skipAnimation);

        public void Hide(Action callback) => _reactivePresenter.Hide(callback);

        public void Hide(bool skipAnimation, Action callback) => _reactivePresenter.Hide(skipAnimation, callback);

        public IObservable<Unit> HideAsObservable(bool skipAnimation = default) =>
            _reactivePresenter.HideAsObservable(skipAnimation);
    }
}
#endif
