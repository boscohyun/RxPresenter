namespace Boscohyun.RxPresenter.Tests.EditMode.Fixtures
{
    public class ViewAnimator : IViewAnimator
    {
        public bool ActiveSelf { get; private set; }
        public bool AnimatorAlwaysActive { get; }
        public ViewAnimatorState CurrentAnimatorState { get; private set; }
        public float CurrentAnimatorStateNormalizedTime { get; private set; }

        public ViewAnimator(bool animatorAlwaysActive = default)
        {
            AnimatorAlwaysActive = animatorAlwaysActive;
        }
        
        public void PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime)
        {
            CurrentAnimatorState = viewAnimatorState;
            CurrentAnimatorStateNormalizedTime = normalizedTime;
        }

        public void SetActive(bool active) => ActiveSelf = active;
    }
}