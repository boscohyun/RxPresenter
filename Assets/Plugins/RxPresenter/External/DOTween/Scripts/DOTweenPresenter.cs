#if RXPRESENTER_DOTWEEN_SUPPORT
using DG.Tweening;

namespace Boscohyun.RxPresenter.External.DOTween
{
    public abstract class DOTweenPresenter : Presenter
    {
        private DOTweenViewAnimator _viewAnimator;

        #region IView

        public override bool HasViewAnimator => _viewAnimator.hasTween;

        public override IViewAnimator ViewAnimator => _viewAnimator;

        #endregion

        protected override void Awake()
        {
            _viewAnimator = new DOTweenViewAnimator(GetShowTween(), GetHideTween());
            base.Awake();
        }

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
