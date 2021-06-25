#if RXPRESENTER_DOTWEEN_SUPPORT
using DG.Tweening;

namespace Boscohyun.RxPresenter.External.DOTween
{
    public abstract class DOTweenReactivePresenter<T> : ReactivePresenter<T> where T : DOTweenReactivePresenter<T>
    {
        private DOTweenViewAnimator _viewAnimator;

        #region IView

        public override bool HasViewAnimator => _viewAnimator.hasTween;

        public override IViewAnimator ViewAnimator => _viewAnimator ??= CreateViewAnimator();

        #endregion

        protected virtual DOTweenViewAnimator CreateViewAnimator() =>
            new DOTweenViewAnimator(GetShowTween(), GetHideTween());

        protected override void ShowAtAwake()
        {
            Hide(true);
            base.ShowAtAwake();
        }

        protected abstract Tween GetShowTween();

        protected abstract Tween GetHideTween();
    }
}
#endif
