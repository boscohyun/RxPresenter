namespace Boscohyun.RxPresenter
{
    public interface IViewAnimator
    {
        int AnimatorActiveDelayFrame { get; }
        
        bool AnimatorAlwaysActive { get; }
        
        ViewAnimatorState CurrentAnimatorState { get; }
        
        float CurrentAnimatorStateNormalizedTime { get; }

        void PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime);
        
        void SetActive(bool active);
    }
}
