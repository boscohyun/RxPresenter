using DG.Tweening;

namespace Boscohyun.RxPresenter.External.DOTween
{
    public class DOTweenViewAnimator : IViewAnimator
    {
        private readonly Tween _showTween;
        private readonly Tween _hideTween;
        private ViewAnimatorState _lastPlayedState;

        public readonly bool hasTween;

        public DOTweenViewAnimator(Tween showTween, Tween hideTween)
        {
            _showTween = showTween;
            _hideTween = hideTween;
            hasTween = !(_showTween is null) ||
                       !(_hideTween is null);
        }

        #region IViewAnimator

        int IViewAnimator.AnimatorActiveDelayFrame => 0;

        bool IViewAnimator.AnimatorAlwaysActive => true;

        ViewAnimatorState IViewAnimator.CurrentAnimatorState => _lastPlayedState;

        float IViewAnimator.CurrentAnimatorStateNormalizedTime
        {
            get
            {
                var currentTween = ((IViewAnimator) this).CurrentAnimatorState == ViewAnimatorState.Show
                    ? _showTween
                    : _hideTween;

                return currentTween.ElapsedPercentage();
            }
        }

        void IViewAnimator.PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime)
        {
            _lastPlayedState = viewAnimatorState;

            if (viewAnimatorState == ViewAnimatorState.Show)
            {
                if (_showTween is null)
                {
                    return;
                }

                var time = _showTween.Duration() * normalizedTime;
                _showTween.Goto(time, true);
            }
            else
            {
                if (_hideTween is null)
                {
                    return;
                }

                var time = _hideTween.Duration() * normalizedTime;
                _hideTween.Goto(time, true);
            }
        }

        void IViewAnimator.SetActive(bool active)
        {
            // NOTE: Do nothing.
            // bool IViewAnimator.AnimatorAlwaysActive => true;
        }

        #endregion
    }
}
