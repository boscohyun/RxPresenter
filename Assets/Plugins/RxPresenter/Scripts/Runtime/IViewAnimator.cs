namespace Boscohyun.RxPresenter
{
    public interface IViewAnimator
    {
        bool AnimatorAlwaysActive { get; }
        
        ViewAnimatorState CurrentAnimatorState { get; }
        
        float CurrentAnimatorStateNormalizedTime { get; }

        void PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime);
        
        void SetActive(bool active);
    }
}
